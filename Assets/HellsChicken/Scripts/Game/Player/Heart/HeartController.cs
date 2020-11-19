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
                Destroy(gameObject);
            }
        }
    }
}
