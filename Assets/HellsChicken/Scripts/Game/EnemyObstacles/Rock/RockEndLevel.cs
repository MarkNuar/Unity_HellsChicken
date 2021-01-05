using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class RockEndLevel : MonoBehaviour {
    
    
        private Rigidbody _rigidbody;
    
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Lava")) {
                _rigidbody.constraints = RigidbodyConstraints.None;
                Destroy(gameObject,10f);
            }
        }
    }
}
