using System;
using System.Collections;
using EventManagerNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

namespace HellsChicken.Scripts.Game.CheckPoint
{
    public class CheckPointManager : MonoBehaviour
    {
        //NEVER PLACE TWO CHECKPOINTS IN THE SAME POSITION!
        public UnityEngine.Light pointLight;

        [SerializeField] private UnityEngine.Light directionalLight;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelManager.Instance.SetCurrentCheckPointPos(transform.position);
                LevelManager.Instance.SetCurrentLightIntensity(directionalLight.intensity);
                //TODO START CHECKPOINT ACTIVATION ANIMATION
                //StartCoroutine(CheckPointActivateLight(2f));
                if(!pointLight.enabled)
                    EventManager.TriggerEvent("checkpointActivation");
                pointLight.enabled = true;
                
            }
        }
    }
}
