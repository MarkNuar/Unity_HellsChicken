using UnityEngine;

namespace HellsChicken.Scripts.Game.CheckPoint
{
    public class EndLevelManager : MonoBehaviour
    {

        [SerializeField] private GameObject endLevelCanvas;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //if boss is killed
                //TODO: NOT INCREASE, BUT MARK THE LEVEL AS COMPLETED
                GameManager.Instance.SetLevelAsCompleted(LevelManager.Instance.levelNumber);
                endLevelCanvas.GetComponent<EndMenu>().Pause();
            }
        }
    }
}
