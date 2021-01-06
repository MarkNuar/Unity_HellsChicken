using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class FireBallAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource fireBallAudioSource;

    [SerializeField] private AudioClip fireBallAudioSound;
    
    protected void Awake() {
        EventManager.StartListening("fireBall", FireBall);
    }

    public void FireBall()
    {
        EventManager.StopListening("fireBall", FireBall);
        fireBallAudioSource.clip = fireBallAudioSound;
        if(!fireBallAudioSource.isPlaying)
            fireBallAudioSource.Play();
        EventManager.StartListening("fireBall", FireBall);
    }
}
