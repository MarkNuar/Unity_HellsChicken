﻿using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles
{
    public class BladeMovement : MonoBehaviour
    {

        [SerializeField] private float rotationSpeed = 5.0f;
        [SerializeField] private float speed = 5.0f;
        
        [SerializeField] Transform startPoint;
        [SerializeField] Transform finishPoint;

        private bool _turnBack;
        
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 0, rotationSpeed);

            if (transform.position.x <= startPoint.position.x)
            {
                _turnBack = false;
            }
            if (transform.position.x >= finishPoint.position.x)
            {
                _turnBack = true;
            }

            if (_turnBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, finishPoint.position, speed * Time.deltaTime);
            }
        }
        }
}