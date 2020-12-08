using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class BladeAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bladeAudioSource;

    [SerializeField] private AudioClip bladeAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("bladeSound", bladeSound);
    }


    public void bladeSound()
    {
        EventManager.StopListening("bladeSound", bladeSound);
        bladeAudioSource.clip = bladeAudioSound;
        bladeAudioSource.loop = true;
        if(!bladeAudioSource.isPlaying)
            bladeAudioSource.Play();
        EventManager.StartListening("bladeSound", bladeSound);
    }
}
