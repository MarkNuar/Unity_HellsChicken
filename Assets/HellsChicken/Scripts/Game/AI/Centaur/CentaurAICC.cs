using System;
using System.Collections;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.AI.Centaur.Bow;
using HellsChicken.Scripts.Game.AI.DecisionTree;
using HellsChicken.Scripts.Game.Player;
using HellsChicken.Scripts.Game.Player.Egg;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HellsChicken.Scripts.Game.AI.Centaur
{
    public class CentaurAICC : MonoBehaviour
    {
        private int _still; //Variable for checking if I have to be still. 
        private bool _canMove = true; //Variable for checking if I can be still. 
        private bool _right = true;
    
        [SerializeField] private float agentVelocity = 8f;
        [SerializeField] private float timeReaction = 2f; //how often do the agent take decision?
        [SerializeField] private Transform player;
        [SerializeField] private Transform arrowPosition;
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private int attackTime;
        public Animator anim;

        [SerializeField] private float gravityScale = 1f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private float yPosition;

        private CharacterController _characterController;
        private Vector3 _movement; 
        private DecisionTree.DecisionTree _tree;
        private GameObject _textInstance;

        private int _shootInterval;

        private bool _isColliding;
        private bool isQuestionMarkTriggered;
        private bool isMoving;
        private bool isShooting;
        private bool isDead;
        
        private PlayerController _playerController;
        
        private readonly RaycastHit[] _feetHitPoints = new RaycastHit[5];
        [SerializeField] private LayerMask slideMask;
        private Vector3 _hitNormal;
        private float _slopeAngle;
    
        private void Awake() 
        {
            _characterController = GetComponent<CharacterController>();
            _playerController = player.gameObject.GetComponent<PlayerController>();
        }

        // Start is called before the first frame update
        void Start() 
        {
            //_characterController.detectCollisions = false;
            _movement = Vector3.zero;
            isQuestionMarkTriggered = true;
            isMoving = false;
            isShooting = false;
            isDead = false;

            //Decision
            DTDecision d1 = new DTDecision(IsPlayerVisible);
            DTDecision d2 = new DTDecision(IsPlayerStill);
        
            //Action
            DTAction a1 = new DTAction(Hit);
            DTAction a2 = new DTAction(Move);
            DTAction a3 = new DTAction(Stop);
        
            d1.AddLink(true,a1);
            d1.AddLink(false,d2);

            d2.AddLink(true, a2);
            d2.AddLink(false, a3);
        
            //root
            _tree = new DecisionTree.DecisionTree(d1);

            StartCoroutine(TreeCoroutine());
        }

        private void Update()
        {
            if(!isDead){
                if (_characterController.enabled)
                {
                    //parallel to ground
                    GroundCheck();
                    float yRotation;
                    float zRotation;
                    if (_right)
                    {
                        yRotation = 0f;
                        zRotation = _hitNormal.x > 0 ? -_slopeAngle : _slopeAngle;
                    }
                    else
                    {
                        yRotation = 180f;
                        zRotation = _hitNormal.x > 0 ? _slopeAngle : -_slopeAngle;
                    }
                    transform.rotation = Quaternion.Euler(transform.rotation.x,yRotation,zRotation);
                    //end parallel to ground
                    
                    if (_characterController.isGrounded)
                        _movement.y = -20f;
                    else
                        _movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;
                    if (_canMove)
                    {
                        if (!_isColliding)
                        {
                            if (_right)
                            {
                                _movement.x = agentVelocity;
                            }
                            else
                            {
                                _movement.x = -agentVelocity;
                            }
                        }
                        else
                        {
                            _movement.x = 0;
                        }
                    }
                    else
                    {
                        _movement.x = 0;
                    }

                    _characterController.Move(_movement * Time.deltaTime);
                }

                if (_textInstance != null && isQuestionMarkTriggered)
                {
                    EventManager.TriggerEvent("centaurQuestionMark");
                    isQuestionMarkTriggered = false;
                }

                if (_textInstance == null)
                    isQuestionMarkTriggered = true;

                if (_movement.x != 0)
                    isMoving = true;
                else
                    isMoving = false;

                anim.SetBool("isMoving", isMoving);
                anim.SetBool("isShooting", isShooting);
            } 
        }
        
        
        //If true, it sets _hitNormal and _slopeAngle variables to the current values.
        private bool GroundCheck()
        {
            Vector3 normalSum = Vector3.zero;
            CharacterController cc = _characterController;
            float carRad = cc.radius;
            Vector3 ccc = cc.center + transform.position;
            var skinWidth = 0.5f;
            Vector3 sourcePoint = new Vector3(ccc.x, ccc.y - (cc.height / 2 - carRad) + skinWidth / 2, ccc.z);
            var numberOfHits = Physics.SphereCastNonAlloc(sourcePoint, carRad, Vector3.down, _feetHitPoints, skinWidth, slideMask);
            if (numberOfHits == 0)
            {
                _hitNormal = Vector3.zero;
                _slopeAngle = 0f;
                return false;
            }
            for(var i = 0; i < numberOfHits; i++)
            {
                normalSum += _feetHitPoints[i].normal;
            }
            _hitNormal = Vector3.ProjectOnPlane(normalSum.normalized, new Vector3(0, 0, 1)).normalized;
            _slopeAngle = Vector3.Angle(Vector3.up, _hitNormal);
            return true;
        }

        private IEnumerator TreeCoroutine() 
        {
            while (!isDead) 
            {
                _tree.Start();
                yield return new WaitForSeconds(timeReaction);
            }
        }
    
        //Actions
        private object Move() 
        {
            _canMove = true;
            _shootInterval = 0;
            isShooting = false;
            EventManager.TriggerEvent("centaurSteps");
            return null;
        }

        private object Stop() 
        {
            _canMove = false;
            _shootInterval = 0;
            isMoving = false;
            EventManager.TriggerEvent("stopCentaurSteps");
            return null;
        }

        private object Hit() 
        {
            if (_shootInterval == 0 && !isDead) 
            {
                GameObject fire = Instantiate(bombPrefab, arrowPosition.position, Quaternion.LookRotation(player.position, transform.position));
                CentaurFire ar = fire.GetComponent<CentaurFire>();
                ar.Target = _playerController.getPredictedPosition(yPosition);//player.position + new Vector3(0, 0.5f, 0);
                ar.CentaurPos = transform.position;
                ar.FindAngle(_right,arrowPosition.position);
                EventManager.TriggerEvent("centaurShot");
                isShooting = true;
            }
            _shootInterval = (_shootInterval + 1) % attackTime;
            return null;
        }

        //Decisions
        private object IsPlayerVisible() 
        { 
            Vector3 ray = player.position - transform.position;
            //Debug.DrawLine(transform.position,player.position, Color.white, 0.5f);
            //if (Physics.Raycast(transform.position, ray, out hit, 30f,mask))
            if(Physics.SphereCast(transform.position,0.5f,ray,out var hit,30,mask))
            {
                if (hit.transform.CompareTag("Player")) 
                {
                    if (Vector3.Dot(ray, transform.right) <= 0) 
                    {
                        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                        _right = !_right;
                    }

                    if (_textInstance != null)
                        Destroy(_textInstance);

                    _canMove = false;
                    _still = 0;
                    return true;
                }
            }

            return false;
        }

        private object IsPlayerStill() 
        {
            if (_still == 0) 
            {
                if (Random.Range(0, 10) > 6) 
                {
                    _still = +1;
                    
                    if(transform.eulerAngles.y>90)
                        _textInstance = Instantiate(textPrefab, gameObject.transform.position + new Vector3(-1.5f, 6f, 0), Quaternion.identity);
                    else 
                        _textInstance = Instantiate(textPrefab, gameObject.transform.position + new Vector3(1.5f, 6f, 0), Quaternion.identity);
                        
                    _textInstance.transform.parent = gameObject.transform.parent;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else 
            {
                if (_still > 0) 
                {
                    _still += 1;
                    if (_still == 4) 
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            transform.rotation *= Quaternion.Euler(0, 180, 0);
                            _right = !_right;
                        }
                        Destroy(_textInstance);
                        _still = -4;
                    }
                    return false;
                }
                else 
                {
                    _still += 1;
                    return true;
                }
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.CompareTag("Player"))
                _isColliding = true;
            
            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy")) 
            {
                transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                _right = !_right;
            }
            
            if(other.gameObject.CompareTag("Attack"))
                TriggerCentaurDeath();

        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player")) 
                _isColliding = false;
        }

        private void OnDestroy() {
            if(_textInstance != null)
                Destroy(_textInstance);                
        }

        public void TriggerCentaurDeath()
        {
            StartCoroutine(CentaurDeath(3f));
        }
        
        IEnumerator CentaurDeath(float time)
        {
            isDead = true;
            transform.parent.GetComponent<CentaurDissolvation>().Dissolvation = true;
            anim.SetBool("isDead",isDead);
            Destroy(_textInstance);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<CharacterController>().enabled = false;
            EventManager.TriggerEvent("centaurDeath");
            yield return null;
        }
    }
}
