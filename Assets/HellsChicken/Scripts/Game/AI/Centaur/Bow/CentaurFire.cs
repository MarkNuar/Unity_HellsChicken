using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Centaur.Bow
{
    [RequireComponent(typeof(Rigidbody))]
    public class CentaurFire : MonoBehaviour {
    
        [SerializeField] private GameObject contactExplosion;
        [SerializeField] private float initialVelocity = 20f;
        [SerializeField] private Animator bowAnimator;
        [SerializeField] private GameObject startExplosion;
    
        private Rigidbody _rigidbody;
        private Vector3 _target;
        private Vector3 _centaurPos;
        private Vector3 _lastPos;
        private bool _collision = true;
    
        public Vector3 Target {
            set => _target = value;
        }
    
        public Vector3 CentaurPos {
            set => _centaurPos = value;
        }
    
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            transform.rotation = LookAt2D(_rigidbody.velocity);
        }
    
        //Quando angolo sopra testa, non funziona.
        //Search for the angle for the trajectory.
        public void FindAngle(bool right,Vector3 arrowPosition) {
            float g = -Physics.gravity.y;
            float v2 = initialVelocity * initialVelocity;
            float v4 = v2 * v2;
            float x = Mathf.Abs((_target.x - _centaurPos.x));
            float y = Mathf.Abs((_target.y - _centaurPos.y));
            float x2 = x * x;
            float squareRoot = Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2));
        
            //This is the angle to be applied from the horizontal vector (but i couldn't get it).
            float angle = Mathf.Atan((v2 - squareRoot) / (g * x))*Mathf.Rad2Deg;

            if (!float.IsNaN(angle)) {

                //Different angle if i am going right or left.
                Vector3 rotatedVector;
                float angleCentaurTarget;
                if (!right) {
                    //Calculate the angle between the horizontal vector (here works) and the vector from the centaur to the target.
                    angleCentaurTarget = Mathf.Acos(Vector3.Dot(Vector3.left, (_target - _centaurPos).normalized)) *
                                         Mathf.Rad2Deg;
                    //Substracted the 2 angle and apply the rotation from the target (not from horizontal).
                    rotatedVector = Quaternion.AngleAxis(angle - angleCentaurTarget, Vector3.back) *
                                    (_target - _centaurPos);
                }
                else {
                    angleCentaurTarget = Mathf.Acos(Vector3.Dot(Vector3.left, (_target - _centaurPos).normalized)) *
                                         Mathf.Rad2Deg;
                    angleCentaurTarget = 180 - angleCentaurTarget;
                    rotatedVector = Quaternion.AngleAxis(angle - angleCentaurTarget, Vector3.forward) *
                                    (_target - _centaurPos);
                }

                rotatedVector += _centaurPos;

                Instantiate(startExplosion, arrowPosition, Quaternion.identity);
                EventManager.TriggerEvent("centaurShot");
                //Apply new velocity
                _rigidbody.velocity = (rotatedVector - _centaurPos).normalized * initialVelocity;
            }
            else
                _collision = false;
        }

        public void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Enemy")) {
                Physics.IgnoreCollision(other.collider, GetComponent<BoxCollider>());
            }else if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall")
                                                            || other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Lava")) {
                Destroy(gameObject);
            }
            if(!other.gameObject.CompareTag("Enemy") && _collision)
                Instantiate(contactExplosion, other.contacts[0].point, Quaternion.identity);
        }
    
        static Quaternion LookAt2D(Vector2 forward) {
            return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
        }
    }
}
