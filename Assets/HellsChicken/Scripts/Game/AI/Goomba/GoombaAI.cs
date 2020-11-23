using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Goomba
{
    [RequireComponent(typeof(Rigidbody))]

/*
 * AI of Goomba. Implemented as continue left - right movement. When he dies, he leaves a bomb that will explode in some seconds.
 */
    public class GoombaAI : MonoBehaviour {
    
        [SerializeField] private bool right = true;
        [SerializeField] private float agentVelocity = 8f;

        private Rigidbody _rigidbody;
        private bool _isColliding;
    
        //da modificare
        private Vector3 _position, _velocity;
    
        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!_isColliding) {
                if (right)
                {
                    _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.right);
                }
                else
                {
                    _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.left);
                }
                _position = _rigidbody.position;
                _velocity = _rigidbody.velocity;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _isColliding = true;
                Physics.IgnoreCollision(other.collider,GetComponent<CapsuleCollider>());
            }
            else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy"))
            {
                right = !right;
                transform.rotation *= Quaternion.Euler(0, 180, 0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                right = !right;
                transform.rotation *= Quaternion.Euler(0, 180, 0);
            }
        }

        private void LateUpdate()
        {
            if (_isColliding)
            {
                _rigidbody.position = _position;
                _rigidbody.velocity = _velocity;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.CompareTag("Player")) 
                _isColliding = false;
        
        }
    }
}
