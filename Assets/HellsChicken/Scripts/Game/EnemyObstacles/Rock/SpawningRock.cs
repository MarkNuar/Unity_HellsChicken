using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class SpawningRock : MonoBehaviour
    {
        private bool _startSpawnRock;

        [SerializeField] private float timerBeforeAnotherRock = 5.0f;
        private float _countdown;

        [SerializeField] private GameObject rockPrefab;
        [SerializeField] private float rockVelocity = 15;
        
        // Update is called once per frame
        private void Update()
        {
            if (_startSpawnRock)
                _countdown -= Time.deltaTime;
            
            if (_countdown <= 0f)
            {
                SpawnRock();
                _countdown = timerBeforeAnotherRock;
            }
        }
        
        private void SpawnRock()
        {
            GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
            rock.GetComponent<Rigidbody>().velocity = new Vector3(-rockVelocity, -rockVelocity, 0f);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
                _startSpawnRock = true;
        }
    }
}
