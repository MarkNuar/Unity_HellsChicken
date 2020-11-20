using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Goomba
{
    public class GoombaAICC : MonoBehaviour
    {
        [SerializeField] private bool right = true;
        [SerializeField] private float gravityScale = 1f;
        private CharacterController _characterController;
        private float agentVelocity = 8f;

        private Vector3 _movement; 
        public void Awake() {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            //_characterController.detectCollisions = false;
            _movement = Vector3.zero; 
        }

        private void Update() {
            _movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;
            if (right)
                _movement.x = agentVelocity;
            else
                _movement.x = -agentVelocity;
            _characterController.Move(_movement * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy")) {
                right = !right;
                transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
