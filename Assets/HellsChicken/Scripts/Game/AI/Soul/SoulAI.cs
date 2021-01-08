using UnityEngine;
using Pathfinding;

namespace HellsChicken.Scripts.Game.AI.Soul
{
    public class SoulAI : MonoBehaviour
    {
        private Vector3 _startingPosition;
        [SerializeField] private float maxDistanceFromStartingPosition = 25f;
        
        [SerializeField] private Transform targetPlayer;
        [SerializeField] private Transform targetStartPosition;
        private bool _followPlayer;
        
        //[SerializeField] private Transform soul;

        [SerializeField] private float speed = 200f;
        [SerializeField] private float d = 20f;
        //how close are enemy need to be to a way point before it moves to the next one
        [SerializeField] private float nextWaypointDistance = 3.5f;

        [SerializeField] private Animator anim;
        
        // private Quaternion _leftRotation;
        // private Quaternion _rightRotation;

        private Path _path;
        private int _currentWaypoint;
        
        private Seeker _seeker;
        private Rigidbody _rb;

        private Vector3 _force;
        
        // Start is called before the first frame update
        private void Start()
        {
            _followPlayer = false;
            
            _startingPosition = transform.position;

            _seeker = GetComponent<Seeker>();
            _rb = GetComponent<Rigidbody>();
            
            // _leftRotation = transform.rotation;
            // _rightRotation = _leftRotation * Quaternion.Euler(0, 180, 0);
            // transform.rotation = _leftRotation;
            
            InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
        }

        private void UpdatePath()
        {
            if (_seeker.IsDone())
            {
                if (_followPlayer)
                {
                    _seeker.StartPath(_rb.position, targetPlayer.position, OnPathComplete);
                }
                else
                {
                    _seeker.StartPath(_rb.position, targetStartPosition.position, OnPathComplete);
                }
            }
                
        }

        private void OnPathComplete(Path p)
        {
            if (p.error) return;
            
            _path = p;
            _currentWaypoint = 1;  //beginning of the new path
        }

        private void FixedUpdate()
        {
            if (_path == null) return;
            
            //reached end of the path
            if (_currentWaypoint >= _path.vectorPath.Count)
                return;

            var position = _rb.position;
            
            Vector2 direction = (_path.vectorPath[_currentWaypoint] - position).normalized;
            _force = direction * (speed * Time.fixedDeltaTime);
            
            if (Vector3.Distance(targetPlayer.position, targetStartPosition.position) > maxDistanceFromStartingPosition)
            {
                _followPlayer = false;
            }
            else
            {
                _followPlayer = true;
            }
            _rb.AddForce(_force);
            
            var distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
            if (distance < nextWaypointDistance)
                _currentWaypoint++;
        }

        public bool IsLookingLeft()
        {
            return _force.x <= -0.01f;
        }

        public bool IsLookingRight()
        {
            return _force.x >= 0.01f;
        }
    }
}
