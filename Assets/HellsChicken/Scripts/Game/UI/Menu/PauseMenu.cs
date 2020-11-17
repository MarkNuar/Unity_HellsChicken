using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;
    public GameObject CommandsMenuUI;

    public GameObject EggCrosshairCanvas;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        CommandsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        EggCrosshairCanvas.SetActive(true);
    }
    
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        EggCrosshairCanvas.SetActive(false);
        
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
}
