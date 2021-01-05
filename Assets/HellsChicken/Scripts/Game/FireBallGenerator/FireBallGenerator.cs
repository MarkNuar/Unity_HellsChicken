using System.Collections;
using UnityEngine;

namespace HellsChicken.Scripts.Game.FireBallGenerator
{
    public class FireBallGenerator : MonoBehaviour {

        [SerializeField] private GameObject fireBall;
        [SerializeField] private float timer;

        private GameObject _lastFireBall;
    
        // Start is called before the first frame update
        void Start() {
            StartCoroutine(StartTimer(timer));
        }

        private void OnTriggerEnter(Collider other) {
            Destroy(_lastFireBall);
            _lastFireBall = Instantiate(fireBall,transform.position + new Vector3(0,1f,0), Quaternion.identity);
        }

        IEnumerator StartTimer(float t) {
            yield return new WaitForSeconds(t);
            _lastFireBall = Instantiate(fireBall,transform.position + new Vector3(0,1f,0), Quaternion.identity);
            yield return null;
        }
    }
}
