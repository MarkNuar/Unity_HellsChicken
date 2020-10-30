using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles
{
    public class FallingRock : MonoBehaviour
    {
        private Rigidbody _rb;
        [SerializeField] GameObject rockPrefab;
        
        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals("PlayerBody"))
            {
                _rb.isKinematic = false;
                Destroy(rockPrefab, 5.0f);
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Equals("PlayerBody"))
            {
                //Debug.Log("Got you!");
            }
        }
    }
}
