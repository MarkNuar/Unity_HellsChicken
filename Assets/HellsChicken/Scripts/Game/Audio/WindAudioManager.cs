using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class WindAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource windAudioSource;

    [SerializeField] private AudioClip windAudioSound;


    protected void Awake() {
        EventManager.StartListening("windSound", WindSound);
    }
    
    private void WindSound()
    {
        EventManager.StopListening("windSound", WindSound);
        windAudioSource.clip = windAudioSound;
        windAudioSource.loop = true;
        if(!windAudioSource.isPlaying)
            windAudioSource.Play();
        EventManager.StartListening("windSound", WindSound);
    }
}
