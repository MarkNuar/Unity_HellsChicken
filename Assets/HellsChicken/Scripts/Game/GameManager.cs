using System.Collections.Generic;
using System.IO;
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

        private const int CurrentNumberOfLevels = 2;
        
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
            public float[] bestTimes;
            
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
        
        // SINGLETON
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                QualitySettings.vSyncCount = 1;
                //Application.targetFrameRate = 60;
                
                // Deserialize settings and level reached
                _gameStatePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "gameState.json";
                Debug.Log(_gameStatePath);
                var loaded = false;
                if (File.Exists(_gameStatePath))
                {
                    // There exists already a previous saved state
                    var reader = new StreamReader(_gameStatePath);
                    _gameState = GameState.CreateFromJsonString(reader.ReadToEnd());
                    if (_gameState != null)
                    {
                        loaded = _gameState.bestTimes.Length == CurrentNumberOfLevels; //check if best times array has the correct lenght
                        //TODO FOR THE FUTURE: if new level added, save previous best times.
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
                        musicVolume = 0,
                        effectsVolume = 0,
                        qualityIndex = 0,
                        isFullScreen = true,
                        screenWidth = Screen.currentResolution.width,
                        screenHeight = Screen.currentResolution.height,
                        antiAlias = true,
                        shadows = true,
                        fpsDisplay = false,
                        levelToBeCompleted = 1,
                        bestTimes = new float[CurrentNumberOfLevels]
                    };
                    for (var i = 0; i < CurrentNumberOfLevels; i++)
                    {
                        _gameState.bestTimes[i] = 0;
                        Debug.Log(_gameState.bestTimes);
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public int GetCurrentNumberOfLevels()
        {
            return CurrentNumberOfLevels;
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
            _currentCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing = !activated ? 0 : AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            _gameState.antiAlias = activated;
        }

        public bool GetAntiAliasing()
        {
            return _gameState.antiAlias;
        }
        
        public void SetShadows(bool activated)
        {
            _currentCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = activated;
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

        public void SetLevelAsCompleted(int levelIndex)
        {
            if(levelIndex == _gameState.levelToBeCompleted) //levels above should not be reachable
                _gameState.levelToBeCompleted++;
            // if (_gameState.levelToBeCompleted > currentNumberOfLevels)
            // {
            //     //TODO: IF LEVEL TO BE COMPLETED >= FINAL LEVEL -> SHOW END SCREEN
            // }
        }

        public int GetLevelToBeCompleted()
        {
            return _gameState.levelToBeCompleted;
        }

        public bool UpdateBestTime(int levelIndex, float newTime)
        {
            if (_gameState.bestTimes[levelIndex - 1] != 0)
            {
                if (newTime < _gameState.bestTimes[levelIndex - 1]) // here levels start counting from 0
                {
                    _gameState.bestTimes[levelIndex - 1] = newTime;
                    return true;
                }
                else
                    return false;
            }
            else //timer is 0, so update 100% 
            {
                _gameState.bestTimes[levelIndex - 1] = newTime;
                return true;
            }
        }

        public float GetBestTime(int levelIndex)
        {
            return _gameState.bestTimes[levelIndex - 1];
        }

        public bool IsBestTimeNonZero(int levelIndex)
        {
            return _gameState.bestTimes[levelIndex - 1] != 0;
        }
        
        private void OnDestroy()
        {
            //Create or update game state
            if (_gameStatePath == null) return;
            var writer = new StreamWriter(_gameStatePath, false);
            writer.WriteLine(JsonUtility.ToJson(_gameState));
            writer.Close();
        }
    }
}
