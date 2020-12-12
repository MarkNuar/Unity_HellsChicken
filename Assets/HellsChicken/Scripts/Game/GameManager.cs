using System;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;
        
        // level reached
        // bool if settings changed
        // settings values
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                //QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = 60;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
    }
}
