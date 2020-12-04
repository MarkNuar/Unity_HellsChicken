using UnityEngine;

namespace HellsChicken.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;

        [SerializeField] private GameObject initialCheckPoint;
        [SerializeField] private UnityEngine.Light initialLight;
        
        private Vector3 _currentCheckPointPos;
        private float _currentLightIntensity;
    
        private Vector3 _startingCheckPointPos;
        private float _startingLightIntensity;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                SetCurrentCheckPointPos(initialCheckPoint.transform.position);
                SetCurrentLightIntensity(initialLight.intensity);
                _startingCheckPointPos = _currentCheckPointPos;
                _startingLightIntensity = _currentLightIntensity;
            }
            else
            {
                Destroy(gameObject);
            }
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
            _currentCheckPointPos = new Vector3(newCheckPointPos.x,newCheckPointPos.y, 0f);
        }

        public void SetCurrentLightIntensity(float intensity)
        {
            _currentLightIntensity = intensity;
        }

        public void ResetGameManagerValues()
        {
            _currentCheckPointPos= _startingCheckPointPos;
            _currentLightIntensity = _startingLightIntensity;
        }
    }
}
