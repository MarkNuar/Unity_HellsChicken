using System.Collections.Generic;
using HellsChicken.Scripts.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
  //public TMP_Dropdown resolutionDropdown;
  Resolution[] resolutions;
  public Camera currentCamera;

  
  [SerializeField] private Slider musicSlider;
  [SerializeField] private Slider effectsSlider;
  [SerializeField] private TMP_Dropdown qualityDropDown;
  [SerializeField] private Toggle fullscreenToggle;
  [SerializeField] private Toggle fpsToggle;
  [SerializeField] private TMP_Dropdown resolutionDropdown;
  [SerializeField] private Toggle antiAliasToggle;
  [SerializeField] private Toggle shadowsToggle;
  

  void Start()
  {
    resolutions = Screen.resolutions;
    resolutionDropdown.ClearOptions();
    List<string> options = new List<string>();
    
    int currentResolutionIndex = 0;
    for (int i = 0; i < resolutions.Length ;i++)
    {
      string option = resolutions[i].width + "x" + resolutions[i].height;
      options.Add(option);
      if (resolutions[i].width == GameManager.Instance.GetScreenWidth() && resolutions[i].height == GameManager.Instance.GetScreenHeight())
      {
        currentResolutionIndex = i;
      }
    }
    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = currentResolutionIndex;
    resolutionDropdown.RefreshShownValue();
    
    
    //TODO: FETCH CURRENT SETTINGS VALUES FROM THE GAME MANAGER
    musicSlider.value = GameManager.Instance.GetMusicVolume();
    effectsSlider.value = GameManager.Instance.GetEffectsVolume();
    qualityDropDown.value = GameManager.Instance.GetQuality();
    fullscreenToggle.isOn = GameManager.Instance.GetFullScreen();
    fpsToggle.isOn = GameManager.Instance.GetFPSDisplay();
    GameManager.Instance.UpdateCurrentCamera(currentCamera);
    antiAliasToggle.isOn = GameManager.Instance.GetAntiAliasing();
    shadowsToggle.isOn = GameManager.Instance.GetShadows();
  }

  public void SetFPSDisplay(bool isDisplayed)
  {
    GameManager.Instance.SetFPSDisplay(isDisplayed);
  }
  
  public void SetMusicVolume(float volume)
  {
    GameManager.Instance.SetMusicVolume(volume);
  }

  public void SetEffectsVolume(float volume)
  {
    GameManager.Instance.SetEffectsVolume(volume);
  }

  public void SetQuality(int qualityIndex)
  {
    GameManager.Instance.SetQuality(qualityIndex);
  }

  public void SetFullscreen(bool isFullscreen)
  {
    GameManager.Instance.SetFullscreen(isFullscreen);
  }

  public void SetResolution(int resolutionIndex)
  {
    //TODO
    Resolution resolution = resolutions[resolutionIndex];
    GameManager.Instance.SetResolution(resolution.width, resolution.height);
  }

  public void SetAntialiasing(bool activated)
  {
    GameManager.Instance.SetAntialiasing(activated);
  }

  public void SetShadows(bool activated)
  {
    GameManager.Instance.SetShadows(activated);
  }
}
