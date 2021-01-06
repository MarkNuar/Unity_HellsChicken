using TMPro;
using UnityEngine;

namespace HellsChicken.Scripts.Game
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
    
        [SerializeField] private GameObject initialCheckPoint;
        [SerializeField] private UnityEngine.Light initialLight;
        public int levelNumber;
        
        private Vector3 _currentCheckPointPos;
        private float _currentLightIntensity;

        public bool isCurrentCkptTheFirst;

        public bool enableFirstCktpCheck = false;
        
        public TextMeshProUGUI timerText; 
        private bool _playing;
        private float _timer;

        public bool isNewBestTime;
        
        // SINGLETON
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                SetCurrentCheckPointPos(initialCheckPoint.transform.position);
                SetCurrentLightIntensity(initialLight.intensity);
                if(enableFirstCktpCheck)
                    isCurrentCkptTheFirst = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            _playing = true;
        }
        
        void Update () {
            if(_playing == true){
                _timer += Time.deltaTime;
                timerText.text = GetFormattedTime(_timer);
            }
        }

        public string GetFormattedTime(float timer)
        {
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer % 60F);
            int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);
            return minutes.ToString ("00") + ":" + seconds.ToString ("00") + ":" + milliseconds.ToString("00");
        }

        public float GetTimer()
        {
            return _timer;
        }

        public void StopTimer()
        {
            _playing = false;
        }
        
        
        public Vector3 GetCurrentCheckPointPos()
        {
            return _currentCheckPointPos;
        }

        public float GetCurrentLightIntensity()
        {
            return _currentLightIntensity;
        }
    
        public void SetCurrentCheckPointPos(Vector3 newCheckPointPos)
        {
            isCurrentCkptTheFirst = false;
            _currentCheckPointPos = new Vector3(newCheckPointPos.x,newCheckPointPos.y, 0f);
        }

        public void SetCurrentLightIntensity(float intensity)
        {
            _currentLightIntensity = intensity;
        }

        public void DestroyLevelManagerInstance()
        {
            Destroy(gameObject);
        }
    }
}
