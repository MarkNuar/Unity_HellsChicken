using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagerNamespace;

public class EggAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bombAudioSource;
    [SerializeField] private AudioSource _bombTimerAudioSource;
    
    [SerializeField] private AudioClip _bombSound;
    [SerializeField] private AudioClip _bombTimerSound;

    private void Awake()
    {
        EventManager.StartListening("playBomb",playBomb);
        EventManager.StartListening("playTimerBomb",playTimerBomb);
    }
    
    private void OnDisable() {
        EventManager.StopListening("playBomb",playBomb);
        EventManager.StopListening("playTimerBomb",playTimerBomb);
    }
    
    public void playBomb(){
        EventManager.StartListening("playBomb",playBomb);
        _bombAudioSource.clip = _bombSound;
        _bombAudioSource.Play();
        EventManager.StopListening("playBomb",playBomb);
    }
    
    public void playTimerBomb(){
        EventManager.StopListening("playTimerBomb",playTimerBomb);
        _bombTimerAudioSource.clip = _bombTimerSound;
        _bombTimerAudioSource.time = 1f;
        _bombTimerAudioSource.Play();
        EventManager.StartListening("playTimerBomb",playTimerBomb);
    }
}
