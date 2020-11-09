using UnityEngine;
using Pathfinding;

namespace HellsChicken.Scripts.Game.AI.Soul
{
    public class SoulAI : MonoBehaviour
    {

        [SerializeField] Transform target;

        private float speed = 1000f;
        private float nextWaypointDistance = 3f;
        
        private Path _path;
        private int _currentWaypoint;
        private bool _reachedEndOfPath;

        private Seeker _seeker;
        private Rigidbody _rb;
        
        // Start is called before the first frame update
        void Start()
        {
            _seeker = GetComponent<Seeker>();
            _rb = GetComponent<Rigidbody>();
            
            InvokeRepeating("UpdatePath", 0f, 0.5f);
        }

        void UpdatePath()
        {
            if (_seeker.IsDone())
            {
                _seeker.StartPath(_rb.position, target.position, OnPathComplete);
            }
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentWaypoint = 0;
            }
        }
        
        void FixedUpdate()
        {
            if (_path == null)
            {
                return;
            }

            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                _reachedEndOfPath = true;
                return;
            }
            else
            {
                _reachedEndOfPath = false;
            }

            Vector2 direction = (_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            //if (target.position.x > transform.position.x && target.rotation.y == 180 || target.position.x < transform.position.x && target.rotation.y == 0)
            {
                _rb.AddForce(force);
            }
            
            float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                _currentWaypoint++;
            }

            if (force.x >= 0.01f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (force.x <= -0.01f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
