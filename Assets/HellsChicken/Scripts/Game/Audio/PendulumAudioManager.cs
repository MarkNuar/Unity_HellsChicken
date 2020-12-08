using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class PendulumAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource pendulumAudioSource;

    [SerializeField] private AudioClip pendulumAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("pendulumSound", pendulumSound);
    }


    public void pendulumSound()
    {
        EventManager.StopListening("pendulumSound", pendulumSound);
        pendulumAudioSource.clip = pendulumAudioSound;
        if(!pendulumAudioSource.isPlaying)
            pendulumAudioSource.Play();
        EventManager.StartListening("pendulumSound", pendulumSound);
    }
}
