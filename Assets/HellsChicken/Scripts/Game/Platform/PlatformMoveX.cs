using System;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform
{
    public class PlatformMoveX : MonoBehaviour
    {
        [SerializeField] private float speed = 5.0f;
        
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform startPoint;
        
        private bool _turnBack;

        public bool startFromStartPoint;

        private void Awake()
        {
            if (startFromStartPoint)
            {
                transform.position = startPoint.position;
            }
            else
            {
                transform.position = endPoint.position;
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (transform.position.x <= endPoint.position.x)
            {
                _turnBack = false;
            }
            
            if (transform.position.x >= startPoint.position.x)
            {
                _turnBack = true;
            }
            
            print(_turnBack);
            transform.position = Vector3.MoveTowards(transform.position, _turnBack ? endPoint.position : startPoint.position, speed * Time.fixedDeltaTime);
        }
    }
}
