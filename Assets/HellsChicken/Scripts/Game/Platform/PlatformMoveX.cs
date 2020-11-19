﻿using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform
{
    public class PlatformMoveX : MonoBehaviour
    {
        private float speed = 5.0f;
        
        [SerializeField] Transform startPoint;
        [SerializeField] Transform finishPoint;

        [SerializeField] GameObject player;

        private bool _turnBack;
        
        // Update is called once per frame
        void Update()
        {
            if (transform.position.x <= startPoint.position.x)
            {
                _turnBack = false;
            }
            if (transform.position.x >= finishPoint.position.x)
            {
                _turnBack = true;
            }

            transform.position = Vector3.MoveTowards(transform.position, _turnBack ? startPoint.position : finishPoint.position, speed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                player.transform.parent = transform;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                player.transform.parent = null;
            }
        }
    }
}
