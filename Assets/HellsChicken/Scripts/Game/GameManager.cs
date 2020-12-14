using System;
using System.IO;
using UnityEditor;
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

        
        [System.Serializable] public class GameState
        {
            public float musicVolume;
            public float effectsVolume;
            public int qualityIndex;
            public bool isFullScreen;
            public int screenWidth;
            public int screenHeight;
            public bool antiAlias;
            public bool shadows;
            public bool fpsDisplay;
            public int levelToBeCompleted; //current level to be completed
            
            public static GameState CreateFromJsonString(string jsonString)
            {
                return JsonUtility.FromJson<GameState>(jsonString);
            }
            public string SaveToJsonString()
            {
                return JsonUtility.ToJson(this);
            }
        }
        
        private GameState _gameState;
        private string _gameStatePath;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                //QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = 60;
                
                
                //TODO deserialize settings and level reached
                _gameStatePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "gameState.json";
                Debug.Log(_gameStatePath);
                bool loaded = false;
                if (System.IO.File.Exists(_gameStatePath))
                {
                    loaded = true;
                    //There exists already a previous saved state
                    var reader = new StreamReader(_gameStatePath);
                    //Debug.Log(reader.ReadToEnd());
                    _gameState = GameState.CreateFromJsonString(reader.ReadToEnd());
                    if (_gameState != null)
                    {
                        loaded = true;
                    }
                    else
                    {
                        Debug.LogError("State not correctly loaded");
                    }
                    
                }
                if(!loaded)
                {
                    _gameState = new GameState
                    {
                        musicVolume = -30f,
                        effectsVolume = -30f,
                        qualityIndex = 0,
                        isFullScreen = true,
                        screenWidth = Screen.currentResolution.width,
                        screenHeight = Screen.currentResolution.height,
                        antiAlias = true,
                        shadows = true,
                        fpsDisplay = false,
                        levelToBeCompleted = 1
                    };

                    //testing
                    // string json = _gameState.SaveToJsonString();
                    // Debug.Log(json);
                    // GameState newGameState = GameState.CreateFromJsonString(json);
                    // Debug.Log("shadows:"+newGameState.shadows);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume",volume);
            _gameState.musicVolume = volume;
        }

        public float GetMusicVolume()
        {
            return _gameState.musicVolume;
        }
        
        public void SetEffectsVolume(float volume)
        {
            audioMixer.SetFloat("EffectsVolume", volume);
            _gameState.effectsVolume = volume;
        }

        public float GetEffectsVolume()
        {
            return _gameState.effectsVolume;
        }
        
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            _gameState.qualityIndex = qualityIndex;
        }

        public int GetQuality()
        {
            return _gameState.qualityIndex;
        }
        
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            _gameState.isFullScreen = isFullscreen;
        }

        public bool GetFullScreen()
        {
            return _gameState.isFullScreen;
        }

        public void SetResolution(int width, int height)
        {
            Screen.SetResolution(width,height, _gameState.isFullScreen);
            _gameState.screenWidth = width;
            _gameState.screenHeight = height;
        }

        public int GetScreenWidth()
        {
            return _gameState.screenWidth;
        }
        
        public int GetScreenHeight()
        {
            return _gameState.screenHeight;
        }
        
        public void SetAntialiasing(bool activated)
        {
            if (!activated)
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing = 0;
            else
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            _gameState.antiAlias = activated;
        }

        public bool GetAntiAliasing()
        {
            return _gameState.antiAlias;
        }
        
        public void SetShadows(bool activated)
        {
            if (!activated)
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = false;
            else
                _currentCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = true;
            _gameState.shadows = activated;
        }

        public bool GetShadows()
        {
            return _gameState.shadows;
        }
        
        public void SetFPSDisplay(bool activation)
        {
            fpsDisplayGui.enabled = activation;
            _gameState.fpsDisplay = activation;
        }

        public bool GetFPSDisplay()
        {
            return _gameState.fpsDisplay;
        }

        public void UpdateCurrentCamera(Camera currentCam) //used by the settings menu to set current camera
        {
            this._currentCamera = currentCam;
            this.SetShadows(_gameState.shadows);
            this.SetAntialiasing(_gameState.antiAlias);
        }

        public void IncreaseLevelToBeCompleted()
        {
            _gameState.levelToBeCompleted++;
            //TODO: IF LEVEL TO BE COMPLETED >= FINAL LEVEL -> SHOW END SCREEN
        }

        public int GetLevelToBeCompleted()
        {
            return _gameState.levelToBeCompleted;
        }
        
        private void OnDestroy()
        {
            //Create or update game state
            var writer = new StreamWriter(_gameStatePath, false);
            writer.WriteLine(JsonUtility.ToJson(_gameState));
            writer.Close();
        }
    }
}
