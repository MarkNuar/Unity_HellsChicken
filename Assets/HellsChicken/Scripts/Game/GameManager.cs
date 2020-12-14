using System;
using EventManagerNamespace;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace HellsChicken.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        private Camera _currentCamera;
        
        public static GameManager Instance;

        public AudioMixer audioMixer;
        [SerializeField] private FPSDisplay fpsDisplayGui;
        
        //GAME STATE
        private float _musicVolume;
        private float _effectsVolume;
        private int _qualityIndex;
        private bool _isFullScreen;
        private Resolution _resolution;
        private bool _antiAlias;
        private bool _shadows;
        private bool _fpsDisplay;
        private int _levelToBeCompleted; //current level to be completed

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                //QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = 60;
                //TODO deserialize settings and level reached
                //mock deserialization, for testing
                _musicVolume = -30f;
                _effectsVolume = -30f;
                _qualityIndex = 2;
                _isFullScreen = true;
                _resolution = Screen.currentResolution;
                _antiAlias = true;
                _shadows = false;
                _fpsDisplay = true;
                _levelToBeCompleted = 1;
                //TODO
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume",volume);
            _musicVolume = volume;
        }

        public float GetMusicVolume()
        {
            return _musicVolume;
        }
        
        public void SetEffectsVolume(float volume)
        {
            audioMixer.SetFloat("EffectsVolume", volume);
            _effectsVolume = volume;
        }

        public float GetEffectsVolume()
        {
            return _effectsVolume;
        }
        
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            _qualityIndex = qualityIndex;
        }

        public int GetQuality()
        {
            return _qualityIndex;
        }
        
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            _isFullScreen = isFullscreen;
        }

        public bool GetFullScreen()
        {
            return _isFullScreen;
        }

        public void SetResolution(Resolution resolution)
        {
            Screen.SetResolution(resolution.width,resolution.height, Screen.fullScreen);
            _resolution = resolution;
        }

        public Resolution GetResolution()
        {
            return _resolution;
        }
        
        public void SetAntialiasing(bool activated)
        {
            if (!activated)
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing = 0;
            else
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            _antiAlias = activated;
        }

        public bool GetAntiAliasing()
        {
            return _antiAlias;
        }
        
        public void SetShadows(bool activated)
        {
            if (!activated)
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = false;
            else
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = true;
            _shadows = activated;
        }

        public bool GetShadows()
        {
            return _shadows;
        }
        
        public void SetFPSDisplay(bool activation)
        {
            fpsDisplayGui.enabled = activation;
            _fpsDisplay = activation;
        }

        public bool GetFPSDisplay()
        {
            return _fpsDisplay;
        }

        public void UpdateCurrentCamera(Camera currentCam) //used by the settings menu to set current camera
        {
            this._currentCamera = currentCam;
            this.SetShadows(_shadows);
            this.SetAntialiasing(_antiAlias);
        }

        public void IncreaseLevelToBeCompleted()
        {
            _levelToBeCompleted++;
        }

        public int GetLevelToBeCompleted()
        {
            return _levelToBeCompleted;
        }
        
        private void OnDestroy()
        {
            //TODO save changed settings and level reached
        }
    }
}
