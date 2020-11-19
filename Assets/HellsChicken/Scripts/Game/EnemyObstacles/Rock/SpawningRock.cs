using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class SpawningRock : MonoBehaviour
    {

        private bool _startSpawnRock;

        private float _countdown;
        [SerializeField] private float timerBeforeAnotherRock = 5.0f;
        
        [SerializeField] private GameObject rockPrefab;

        private Vector3 _velocity;
        private float rockVelocity = 15;
        
        // Start is called before the first frame update
        void Start()
        {
            _countdown = timerBeforeAnotherRock;
            _velocity = new Vector3(-rockVelocity, -rockVelocity, 0f);
        }

        // Update is called once per frame
        void Update()
        {
            if (_startSpawnRock)
            {
                _countdown -= Time.deltaTime;
            }

            if (_countdown <= 0f)
            {
                SpawnRock();
                _countdown = timerBeforeAnotherRock;
            }
        }
        
        private void SpawnRock()
        {
            GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
            rock.GetComponent<Rigidbody>().velocity = _velocity;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _startSpawnRock = true;
            }
        }
    }
}
