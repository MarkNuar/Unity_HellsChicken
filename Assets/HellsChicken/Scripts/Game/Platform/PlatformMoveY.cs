using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform
{
    public class PlatformMoveY : MonoBehaviour
    {
        private float speed = 5.0f;
        
        [SerializeField] Transform startPoint;
        [SerializeField] Transform finishPoint;

        [SerializeField] GameObject player;

        private bool _turnback;
        
        // Update is called once per frame
        void Update()
        {
            if (transform.position.y <= startPoint.position.y)
            {
                _turnback = false;
            }
            if (transform.position.y >= finishPoint.position.y)
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
            if (other.gameObject.name.Equals("PlayerBody"))
            {
                Debug.Log("eih");
            }
            
            
            if (other.gameObject == player)
            {
                player.transform.parent = transform;
                Debug.Log("ciao");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                player.transform.parent = null;
            }
        }
    }
}