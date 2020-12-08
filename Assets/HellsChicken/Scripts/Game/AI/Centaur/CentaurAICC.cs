using System;
using System.Collections;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.AI.Centaur.Bow;
using HellsChicken.Scripts.Game.AI.DecisionTree;
using HellsChicken.Scripts.Game.Player;
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

        [SerializeField] private float gravityScale = 1f;
        [SerializeField] private LayerMask mask;

        private CharacterController _characterController;
        private Vector3 _movement; 
        private DecisionTree.DecisionTree _tree;
        private GameObject _textInstance;

        private int _shootInterval;

        private bool _isColliding;

        private PlayerController _playerController;
    
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
            if (_characterController.enabled)
            {
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
        }

        private IEnumerator TreeCoroutine() 
        {
            while (true) 
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
            return null;
        }

        private object Stop() 
        {
            _canMove = false;
            _shootInterval = 0;
            return null;
        }

        private object Hit() 
        {
            if (_shootInterval == 0) 
            {
                GameObject fire = Instantiate(bombPrefab, arrowPosition.position, Quaternion.LookRotation(player.position, transform.position));
                CentaurFire ar = fire.GetComponent<CentaurFire>();
                ar.Target = _playerController.getPredictedPosition();//player.position + new Vector3(0, 0.5f, 0);
                ar.CentaurPos = transform.position;
                ar.FindAngle(_right,arrowPosition.position);
                EventManager.TriggerEvent("centaurShot");
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
                        EventManager.TriggerEvent("changeBowDirection");
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
                    _textInstance = Instantiate(textPrefab, gameObject.transform.position + new Vector3(-0.4f, 3, 0),
                        Quaternion.identity);
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
                            EventManager.TriggerEvent("changeBowDirection");
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
                EventManager.TriggerEvent("changeBowDirection");
                _right = !_right;
            }
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
    }
}
