using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class CheckPointAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource checkpointAudioSource;

    [SerializeField] private AudioClip checkpointAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("checkpointActivation", checkpointSound);
    }


    public void checkpointSound()
    {
        EventManager.StopListening("checkpointActivation", checkpointSound);
        checkpointAudioSource.clip = checkpointAudioSound;
        checkpointAudioSource.Play();
        EventManager.StartListening("checkpointActivation", checkpointSound);
    }
}
