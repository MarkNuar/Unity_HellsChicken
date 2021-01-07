using HellsChicken.Scripts.Game.UI.Menu;
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
                GameManager.Instance.SetLevelAsCompleted(LevelManager.Instance.levelNumber);
                LevelManager.Instance.StopTimer();
                LevelManager.Instance.isNewBestTime = GameManager.Instance.UpdateBestTime(
                    LevelManager.Instance.levelNumber,
                    LevelManager.Instance.GetTimer());
                endLevelCanvas.GetComponent<EndMenu>().EndLevel();
            }
        }
    }
}
