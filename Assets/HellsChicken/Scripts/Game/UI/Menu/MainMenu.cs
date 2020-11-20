using UnityEngine;
using UnityEngine.SceneManagement;

namespace HellsChicken.Scripts.Game.UI.Menu
{
   public class MainMenu : MonoBehaviour
   {
      public void PlayGame()
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }

      public void QuitGame()
      {
         Application.Quit();
      }
   }
}
