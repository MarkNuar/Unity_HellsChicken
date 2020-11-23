using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class FallingRock : MonoBehaviour
    {
        private Rigidbody _rockRb;
        [SerializeField] private GameObject rockPrefab;
        
        [SerializeField] private float gravityModifier = 10;

        private void Start()
        {
            _rockRb = rockPrefab.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                _rockRb.isKinematic = false;
                _rockRb.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
            }
        }
    }
}

