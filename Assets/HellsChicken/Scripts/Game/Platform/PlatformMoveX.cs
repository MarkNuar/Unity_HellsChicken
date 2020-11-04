using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform
{
    public class PlatformMoveX : MonoBehaviour
    {
        private float speed = 5.0f;
        
        [SerializeField] Transform startPoint;
        [SerializeField] Transform finishPoint;

        [SerializeField] GameObject player;

        private bool _turnback;
        
        // Update is called once per frame
        void Update()
        {
            if (transform.position.x <= startPoint.position.x)
            {
                _turnback = false;
            }
            if (transform.position.x >= finishPoint.position.x)
            {
                _turnback = true;
            }

            if (_turnback)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, finishPoint.position, speed * Time.deltaTime);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                player.transform.parent = transform;
                Debug.Log("ciao");
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
