using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class RockObstacle : MonoBehaviour
    {

        [SerializeField] private GameManager rockPrefab;
    
        private void OnCollisionEnter(Collision other)
        {
            //TODO: IF HIT BY THE EGG, IT SHOULD FALL WITHOUT DESTROYING ITSELF
            //if (other.collider.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
