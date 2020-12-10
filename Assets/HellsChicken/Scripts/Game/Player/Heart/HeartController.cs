using System.Collections;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Heart
{
    public class HeartController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.TriggerEvent("IncreasePlayerHealth");
                EventManager.TriggerEvent("chickenThigh");
                gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                gameObject.GetComponent<SphereCollider>().enabled = false;
                StartCoroutine(DelayDestroyChickenThigh(1.0f));
            }
        }

        private IEnumerator DelayDestroyChickenThigh(float time)
        {
            Destroy(gameObject);
            yield return new WaitForSeconds(time);
        }
    }
}
