using UnityEngine;
using Pathfinding;

namespace HellsChicken.Scripts.Game.AI.Soul
{
    public class SoulAI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform soul;

        [SerializeField] private float speed = 200f;
        //how close are enemy need to be to a way point before it moves to the next one
        [SerializeField] private float nextWaypointDistance = 3f;

        private Path _path;
        private int _currentWaypoint;
        
        private Seeker _seeker;
        private Rigidbody _rb;
        
        // Start is called before the first frame update
        private void Start()
        {
            _seeker = GetComponent<Seeker>();
            _rb = GetComponent<Rigidbody>();
            
            InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
        }

        private void UpdatePath()
        {
            if (_seeker.IsDone()) 
                _seeker.StartPath(_rb.position, target.position, OnPathComplete);
        }

        private void OnPathComplete(Path p)
        {
            if (p.error) return;
            
            _path = p;
            _currentWaypoint = 0;  //beginning of the new path
        }

        private void FixedUpdate()
        {
            if (_path == null) return;
            
            //reached end of the path
            if (_currentWaypoint >= _path.vectorPath.Count)
                return;
                
            Vector2 direction = (_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
            var force = direction * speed * Time.deltaTime;
                
            //TODO
            //if (target.position.x > transform.position.x && target.rotation.y == 180 || target.position.x < transform.position.x && target.rotation.y == 0)
            {
                _rb.AddForce(force);
            }
                
            var distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
            if (distance < nextWaypointDistance)
                _currentWaypoint++;
                
            //se all'inizio guarda a sinistra
            if (force.x >= 0.01f)
            {
                soul.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (force.x <= -0.01f)
            {
                soul.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.transform.CompareTag("Player"))
        //         Destroy(gameObject);
        // }
    }
}
