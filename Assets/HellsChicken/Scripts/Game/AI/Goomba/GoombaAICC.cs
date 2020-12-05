using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Goomba
{
    public class GoombaAICC : MonoBehaviour
    {
        [SerializeField] private bool right = true;
        [SerializeField] private float gravityScale = 10f;
        [SerializeField] private float agentVelocity = 8f;
        
        private CharacterController _characterController;
        private Vector3 _movement;
      
        public void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            //_characterController.detectCollisions = false;
            _movement = Vector3.zero; 
        }

        private void Update()
        {
            if (_characterController.enabled)
            {
                if (_characterController.isGrounded)
                    _movement.y = -20f;
                else
                    _movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;
                if (right)
                {
                    _movement.x = agentVelocity;
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    _movement.x = -agentVelocity;
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                }

                _characterController.Move(_movement * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("deadEnemy"))
            {
                right = !right;
                transform.rotation *= Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
