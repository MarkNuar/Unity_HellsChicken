using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class RockObstacle : MonoBehaviour
    {

        [SerializeField] private GameManager rockPrefab;
    
        private void OnCollisionEnter(Collision other)
        {
            //if (other.collider.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
