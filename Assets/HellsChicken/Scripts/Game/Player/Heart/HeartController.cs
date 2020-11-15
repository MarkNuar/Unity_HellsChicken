using EventManagerNamespace;
using UnityEngine;

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
