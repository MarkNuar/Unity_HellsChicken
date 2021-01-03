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
    
    private float _yStart;
    private float _yEnd;
    private bool _enabled;

    // Start is called before the first frame update
    void Start()
    {
        _yStart = startPosition.position.y;
        _yEnd = endPosition.position.y;
        _enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enabled)
        {
            var yPlayer = player.position.y;
            if (turnOff)
            {
                if (yPlayer > _yStart)
                {
                    directionalLight.intensity = maxIntensity;
                }
                else if (yPlayer < _yEnd)
                {
                    directionalLight.intensity = minIntensity;
                }
                else
                {
                    directionalLight.intensity = maxIntensity +
                                                 (minIntensity - maxIntensity) * (player.position.y - _yStart) /
                                                 (_yEnd - _yStart);
                }
            }
            else
            {
                if (yPlayer < _yStart)
                {
                    directionalLight.intensity = minIntensity;
                }
                else if (yPlayer > _yEnd)
                {
                    directionalLight.intensity = maxIntensity;
                }
                else
                {
                    directionalLight.intensity = minIntensity +
                                                 (maxIntensity - minIntensity) * (player.position.y - _yStart) /
                                                 (_yEnd - _yStart);
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _enabled = false;
    }
}
