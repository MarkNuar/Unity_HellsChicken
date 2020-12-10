using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class ChickenThighAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource chickenThighAudioSource;

    [SerializeField] private AudioClip chickenThighAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("chickenThigh", chickenThigh);
    }


    public void chickenThigh()
    {
        EventManager.StopListening("chickenThigh", chickenThigh);
        chickenThighAudioSource.clip = chickenThighAudioSound;
        chickenThighAudioSource.Play();
        EventManager.StartListening("chickenThigh", chickenThigh);
    }
}
