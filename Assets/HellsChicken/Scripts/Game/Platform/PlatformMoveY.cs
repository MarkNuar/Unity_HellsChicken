using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform
{
    public class PlatformMoveY : MonoBehaviour
    {
        [SerializeField] private float speed = 5.0f;
        
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform finishPoint;

        [SerializeField] private GameObject player;

        private bool _turnBack;
        
        // Update is called once per frame
        private void Update()
        {
            if (transform.position.y <= startPoint.position.y)
                _turnBack = false;
            
            if (transform.position.y >= finishPoint.position.y)
                _turnBack = true;
            
            transform.position = Vector3.MoveTowards(transform.position, _turnBack ? startPoint.position : finishPoint.position, speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player")) 
                player.transform.parent = transform;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player")) 
                player.transform.parent = null;
        }
    }
}