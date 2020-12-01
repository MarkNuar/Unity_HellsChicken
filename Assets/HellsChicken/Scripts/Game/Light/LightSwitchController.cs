using System;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Light
{
    public class LightSwitchController : MonoBehaviour
    {

        [SerializeField] private bool turnOn = false;
        [SerializeField] private float minIntensity = 0.3f;
        [SerializeField] private float maxIntensity = 3f;
        [SerializeField] private UnityEngine.Light directionalLight;

        private bool _start = false;
        private float _t = 0f;
        private void Update()
        {
            if (_start)
            {
                if (turnOn)
                {
                    directionalLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, _t);
                }
                else
                {
                    directionalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, _t);
                }
                _t += 0.5f * Time.deltaTime;
                if (_t > 1.0f)
                {
                    turnOn = !turnOn;
                    _start = false;
                    _t = 0f;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //TODO SHOULD BE ABLE TO GO BE CHANGED MORE THAN ONCE... 
                _start = true;
            }
        }
    }
}
