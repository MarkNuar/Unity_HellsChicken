using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class SpearAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource spearAudioSource;

    [SerializeField] private AudioClip spearAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("spearSound", spearSound);
    }


    public void spearSound()
    {
        EventManager.StopListening("spearSound", spearSound);
        spearAudioSource.clip = spearAudioSound;
        if(!spearAudioSource.isPlaying)
            spearAudioSource.Play();
        EventManager.StartListening("spearSound", spearSound);
    }
}
