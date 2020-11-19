using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class RockObstacle : MonoBehaviour
    {
        private Rigidbody _rockRb;
        
        [SerializeField] private float gravityModifier = 10;
        
        void Start()
        {
            _rockRb = GetComponent<Rigidbody>();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Attack"))
            {
                _rockRb.isKinematic = false;
                _rockRb.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
            }
            else 
            {
                Destroy(gameObject);
            }
            
            //Destroy(gameObject);
        }
    }
}
