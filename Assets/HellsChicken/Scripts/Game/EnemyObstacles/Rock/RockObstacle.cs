using System.Collections;
using EventManagerNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class RockObstacle : MonoBehaviour
    {
        private Rigidbody _rockRb;
        public Animator anim;
        [SerializeField] private float gravityModifier = 10;

        private void Start()
        {
            _rockRb = GetComponent<Rigidbody>();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Attack"))
            {
                _rockRb.isKinematic = false;
                _rockRb.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
            }
            else 
            {
                anim.SetBool("hasCollided",true);
                StartCoroutine(DestroyGameObject(5.0f));
                transform.gameObject.tag = "deadEnemy";
                gameObject.layer = 21;
                EventManager.TriggerEvent("rockSound");
            }

        }

        IEnumerator DestroyGameObject(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
