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
    
        // SINGLETON
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                SetCurrentCheckPointPos(initialCheckPoint.transform.position);
                SetCurrentLightIntensity(initialLight.intensity);
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

        public void DestroyLevelManagerInstance()
        {
            Destroy(gameObject);
        }
    }
}
