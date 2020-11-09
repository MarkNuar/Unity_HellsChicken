using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles
{
    public class FallingRock : MonoBehaviour
    {
        private Rigidbody _rockRb;
        [SerializeField] private GameObject rockPrefab;
        [SerializeField] private float gravityModifier = 10;
        
        // Start is called before the first frame update
        void Start()
        {
            _rockRb = rockPrefab.GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                _rockRb.isKinematic = false;
                _rockRb.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
            }
        }
        
        void OnCollisionEnter(Collision other)
        {
            /*
            if (other.gameObject.CompareTag("Attack"))
            {
                _rockRb.isKinematic = false;
                _rockRb.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
            */
            
            Destroy(rockPrefab);
        }
    }
}
