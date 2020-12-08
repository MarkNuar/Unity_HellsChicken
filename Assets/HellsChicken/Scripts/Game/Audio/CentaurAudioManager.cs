using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class CentaurAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource centaurShotAudioSource;

    [SerializeField] private AudioClip centaurShotAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("centaurShot", centaurShot);
    }


    public void centaurShot()
    {
        EventManager.StopListening("centaurShot", centaurShot);
        centaurShotAudioSource.clip = centaurShotAudioSound;
        centaurShotAudioSource.Play();
        EventManager.StartListening("centaurShot", centaurShot);
    }
}
