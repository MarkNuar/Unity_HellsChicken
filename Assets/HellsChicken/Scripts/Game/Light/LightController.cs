using System.Collections;
using System.Collections.Generic;
using HellsChicken.Scripts.Game;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    
    // Start is called before the first frame update
    void Start()
    {
        //TODO
        directionalLight.intensity = LevelManager.Instance.GetCurrentLightIntensity();
    }
}