using UnityEngine;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class TimerUI : MonoBehaviour
    {
    
        public static TimerUI Instance;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void DestroyTimerUI()
        {
            Destroy(gameObject);
        }
    }
}
