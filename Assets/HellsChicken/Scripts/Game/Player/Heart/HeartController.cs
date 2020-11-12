using EventManagerNamespace;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Increasing player health");
            EventManager.TriggerEvent("IncreasePlayerHealth");
            Destroy(gameObject);
        }
    }
}
