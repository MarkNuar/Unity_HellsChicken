using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class RockAudioSource : MonoBehaviour
{
    [SerializeField] private AudioSource rockAudioSource;

    [SerializeField] private AudioClip rockAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("rockSound", rockSound);
    }


    public void rockSound()
    {
        EventManager.StopListening("rockSound", rockSound);
        rockAudioSource.clip = rockAudioSound;
        rockAudioSource.Play();
        EventManager.StartListening("rockSound", rockSound);
    }
}
