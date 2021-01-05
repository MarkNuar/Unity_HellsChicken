using UnityEngine;

namespace HellsChicken.Scripts.Game.FireBallGenerator
{
    public class FireBallForce : MonoBehaviour {

        [SerializeField] private float force = 10;

        private Rigidbody _rigidBody;
    
        private void Awake() {
            _rigidBody = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start() {
            _rigidBody.AddForce(Vector3.up * force,ForceMode.Acceleration);
        }
    }
}
