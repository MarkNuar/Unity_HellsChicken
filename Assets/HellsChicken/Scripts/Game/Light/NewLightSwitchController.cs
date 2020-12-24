using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLightSwitchController : MonoBehaviour
{
    
    [SerializeField] private bool turnOff = true;
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float maxIntensity = 3f;
    [SerializeField] private UnityEngine.Light directionalLight;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform player;
    
    private float yStart;
    private float yEnd;
    private bool enabled;

    // Start is called before the first frame update
    void Start()
    {
        yStart = startPosition.position.y;
        yEnd = endPosition.position.y;
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            var yPlayer = player.position.y;
            if (turnOff)
            {
                if (yPlayer > yStart)
                {
                    directionalLight.intensity = maxIntensity;
                }
                else if (yPlayer < yEnd)
                {
                    directionalLight.intensity = minIntensity;
                }
                else
                {
                    directionalLight.intensity = maxIntensity +
                                                 (minIntensity - maxIntensity) * (player.position.y - yStart) /
                                                 (yEnd - yStart);
                }
            }
            else
            {
                if (yPlayer < yStart)
                {
                    directionalLight.intensity = minIntensity;
                }
                else if (yPlayer > yEnd)
                {
                    directionalLight.intensity = maxIntensity;
                }
                else
                {
                    directionalLight.intensity = minIntensity +
                                                 (maxIntensity - minIntensity) * (player.position.y - yStart) /
                                                 (yEnd - yStart);
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        enabled = false;
    }
}
