using UnityEngine;

public class EndLevelManager : MonoBehaviour
{

    [SerializeField] private GameObject endLevelCanvas;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if boss is killed
            endLevelCanvas.GetComponent<EndMenu>().Pause();
        }
    }
}
