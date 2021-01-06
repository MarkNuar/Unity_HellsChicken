using System.Collections;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.FireBallGenerator
{
    public class FireBallGenerator : MonoBehaviour {

        [SerializeField] private GameObject fireBall;
        [SerializeField] private float timer;

        public float timeBetweenMeteors;
        public float throwForce = 1f;
        
        private GameObject _lastFireBall;
    
        // Start is called before the first frame update
        void Start() {
            StartCoroutine(StartTimer(timer));
        }

        private void OnTriggerEnter(Collider other) {
            Destroy(_lastFireBall);
            StartCoroutine(SpawnMeteor(timeBetweenMeteors));
        }

        private IEnumerator SpawnMeteor(float t)
        {
            yield return new WaitForSeconds(t);
            _lastFireBall = Instantiate(fireBall,transform.position + new Vector3(0,throwForce,0), Quaternion.identity);
            EventManager.TriggerEvent("fireBall");
        }
        
        private IEnumerator StartTimer(float t) {
            yield return new WaitForSeconds(t);
            _lastFireBall = Instantiate(fireBall,transform.position + new Vector3(0,throwForce,0), Quaternion.identity);
            yield return null;
        }
    }
}
