using System.Collections;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.AI.Centaur.Bow;
using HellsChicken.Scripts.Game.AI.DecisionTree;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HellsChicken.Scripts.Game.AI.Centaur
{
    [RequireComponent(typeof(Rigidbody))]

    public class CentaurAI : MonoBehaviour {

        private int _still; //Variable for checking if I have to be still. 
        private bool _movement = true; //Variable for checking if I can be still. 
        private bool _right = true;
    
        [SerializeField] private float agentVelocity = 8f;
        [SerializeField] private float timeReaction = 2f; //how often do the agent take decision?
        [SerializeField] private Transform player;
        [SerializeField] private Transform arrowPosition;
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private GameObject startExplosion;
        [SerializeField] private int attackTime;

        private Rigidbody _rigidbody;
        private DecisionTree.DecisionTree _tree;
        private GameObject _textInstance;

        private int _shootInterval;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start() {

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
    
        private void FixedUpdate() {
            if (_movement) {
                if(_right)
                    _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.right);
                else
                    _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.left);
            }else {
                _rigidbody.velocity = Vector3.zero;
            }
        }
    
        IEnumerator TreeCoroutine() {
            while (true) {
                _tree.Start();
                yield return new WaitForSeconds(timeReaction);
            }
        }
    
        //Actions
        private object Move() {
            _movement = true;
            _shootInterval = 0;
            return null;
        }

        private object Stop() {
            _movement = false;
            _shootInterval = 0;
            return null;
        }

        private object Hit() {
            if (_shootInterval == 0) {
                Instantiate(startExplosion, arrowPosition.position, Quaternion.identity);
                EventManager.TriggerEvent("centaurShot");
                GameObject fire = Instantiate(bombPrefab, arrowPosition.position,
                    Quaternion.LookRotation(player.position, transform.position));
                CentaurFire ar = fire.GetComponent<CentaurFire>();
                ar.Target = player.position + new Vector3(0, 0.5f, 0);
                ar.CentaurPos = transform.position;
                //ar.findAngle(right);
            }
            _shootInterval = (_shootInterval + 1) % attackTime;
            return null;
        }

        //Decisions
        private object IsPlayerVisible() { 
            Vector3 ray = player.position - transform.position;
            if (Physics.Raycast(transform.position, ray, out var hit, 30f)) {
                if (hit.transform == player) {
                    if (Vector3.Dot(ray, transform.right) <= 0) {
                        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                        EventManager.TriggerEvent("changeBowDirection");
                        _right = !_right;
                    }

                    if (_textInstance != null)
                        Destroy(_textInstance);

                    _movement = false;
                    _still = 0;
                    return true;
                }
            }

            return false;
        }

        private object IsPlayerStill() {
            if (_still == 0) {
                if (Random.Range(0, 10) > 6) {
                    _still = +1;
                    _textInstance = Instantiate(textPrefab, gameObject.transform.position + new Vector3(-0.4f, 3, 0),
                        Quaternion.identity);
                    return false;
                }
                else
                    return true;

            }else {
                if (_still > 0) {
                    _still += 1;
                    if (_still == 4) {
                        if (Random.Range(0, 2) == 0) {
                            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                            EventManager.TriggerEvent("changeBowDirection");
                            _right = !_right;
                        }
                        Destroy(_textInstance);
                        _still = -4;
                    }
                    return false;
                }else {
                    _still += 1;
                    return true;
                }
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Wall")) {
                transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                EventManager.TriggerEvent("changeBowDirection");
                _right = !_right;
            }

            if (other.gameObject.CompareTag("Player")) {
                Physics.IgnoreCollision(other.collider, GetComponent<CapsuleCollider>());
            }
        }
    
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Wall")) {
                transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                EventManager.TriggerEvent("changeBowDirection");
                _right = !_right;
            }
        }
    }
}
