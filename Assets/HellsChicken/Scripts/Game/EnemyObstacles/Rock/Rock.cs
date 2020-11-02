using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class Rock : MonoBehaviour
    {
        private Rigidbody _rockRb;
        [SerializeField] private GameObject rockObstacle;
        [SerializeField] private float gravityModifier = 10;
        
        //[SerializeField] GameObject rockPrefab;
        
        void Start()
        {
            _rockRb = rockObstacle.GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                _rockRb.isKinematic = false;
                _rockRb.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
                
                //Destroy(rockPrefab, 3.0f);
            }
        }
    }
}

