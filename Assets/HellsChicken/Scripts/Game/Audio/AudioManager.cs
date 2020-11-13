using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField] private AudioSource _bombAudioSource;
    [SerializeField] private AudioSource _bombTimerAudioSource;
    [SerializeField] private AudioSource _centaurShotAudioSource;
    [SerializeField] private AudioSource _flameThrowerAudioSource;
    [SerializeField] private AudioSource _footStepsAudioSource;
    
    [SerializeField] private AudioClip _bombSound;
    [SerializeField] private AudioClip _bombTimerSound;
    [SerializeField] private AudioClip _centaurShotSound;
    [SerializeField] private AudioClip _flameThrowerSound;
    [SerializeField] private AudioClip _footStepsSound;

    protected override void Awake() {
        EventManager.StartListening("playBomb",playBomb);
        EventManager.StartListening("playTimerBomb",playTimerBomb);
        EventManager.StartListening("centaurShot",centaurShot);
        EventManager.StartListening("flameThrower", flameThrower);
        EventManager.StartListening("footSteps", footSteps);
    }

    private void OnDisable() {
        EventManager.StopListening("playBomb",playBomb);
        EventManager.StopListening("playTimerBomb",playTimerBomb);
        EventManager.StopListening("centaurShot",centaurShot);
    }

    public void playBomb(){
        EventManager.StartListening("playBomb",playTimerBomb);
        _bombAudioSource.clip = _bombSound;
        _bombAudioSource.Play();
        EventManager.StopListening("playBomb",playTimerBomb);
    }
    
    public void playTimerBomb(){
        EventManager.StopListening("playTimerBomb",playTimerBomb);
        _bombTimerAudioSource.clip = _bombTimerSound;
        _bombTimerAudioSource.time = 1f;
        _bombTimerAudioSource.Play();
        EventManager.StartListening("playTimerBomb",playTimerBomb);
    }
    
    public void centaurShot() {
        EventManager.StopListening("centaurShot",centaurShot);
        _centaurShotAudioSource.clip = _centaurShotSound;
        _centaurShotAudioSource.Play();
        EventManager.StartListening("centaurShot",centaurShot);
    }

    public void flameThrower()
    {
        EventManager.StopListening("flameThrower", flameThrower);
        _flameThrowerAudioSource.clip = _flameThrowerSound;
        _flameThrowerAudioSource.Play();
        EventManager.StartListening("flameThrower", flameThrower);

    }

    public void footSteps()
    {
        EventManager.StopListening("footSteps", footSteps);
        _footStepsAudioSource.clip = _footStepsSound;
        if(!_footStepsAudioSource.isPlaying)
            _footStepsAudioSource.Play();
        EventManager.StartListening("footSteps",footSteps);
    }
}
