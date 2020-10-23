using System.Collections;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Goomba
{
    public class BombExplosion : MonoBehaviour
    {

        [SerializeField] private GameObject explosionPrefab;

        // Start is called before the first frame update
        void Start() {
            StartCoroutine(MakeExplosion());
        }
    
        //Wait for 2 second and then make the bomb explode.
        IEnumerator MakeExplosion() {  
            yield return new WaitForSeconds(2);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            EventManager.TriggerEvent("playBomb");
            Destroy(gameObject);
            yield return null;
        }
    
    }
}
