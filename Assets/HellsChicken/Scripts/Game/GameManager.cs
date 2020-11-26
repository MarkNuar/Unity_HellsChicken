using UnityEngine;

namespace HellsChicken.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;

        [SerializeField] private GameObject initialCheckPoint;
    
        private Vector3 _currentCheckPointPos;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                SetCurrentCheckPointPos(initialCheckPoint.transform.position);
                //Destroy(initialCheckPoint);//maybe initial checkpoint has to be destroyed after consumed.
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
    
        public void SetCurrentCheckPointPos(Vector3 newCheckPointPos)
        {
            _currentCheckPointPos = newCheckPointPos;
        }
    }
}
