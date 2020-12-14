using HellsChicken.Scripts.Game;
using UnityEngine;

public class EndLevelManager : MonoBehaviour
{

    [SerializeField] private GameObject endLevelCanvas;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if boss is killed
            GameManager.Instance.IncreaseLevelToBeCompleted();
            endLevelCanvas.GetComponent<EndMenu>().Pause();
        }
    }
}
