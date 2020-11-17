﻿using System;
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
    [SerializeField] private AudioSource _wingsFlappingAudioSource;
    [SerializeField] private AudioSource _chickenDamageAudioSource;
    [SerializeField] private AudioSource _chickenDeathAudioSource;


    [SerializeField] private AudioClip _bombSound;
    [SerializeField] private AudioClip _bombTimerSound;
    [SerializeField] private AudioClip _centaurShotSound;
    [SerializeField] private AudioClip _flameThrowerSound;
    [SerializeField] private AudioClip _footStepsSound;
    [SerializeField] private AudioClip _wingsFlappingSound;
    [SerializeField] private AudioClip _chickenDamageSound;
    [SerializeField] private AudioClip _chickenDeathSound;

    protected override void Awake() {
        EventManager.StartListening("playBomb",playBomb);
        EventManager.StartListening("playTimerBomb",playTimerBomb);
        EventManager.StartListening("centaurShot",centaurShot);
        EventManager.StartListening("flameThrower", flameThrower);
        EventManager.StartListening("footSteps", footSteps);
        EventManager.StartListening("stopFootSteps",stopFootSteps);
        EventManager.StartListening("wingsFlap", wingsFlap);
        EventManager.StartListening("chickenDamage",chickenDamage);
        EventManager.StartListening("chickenDeath",chickenDeath);
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
    
    public void stopFootSteps()
    {
        EventManager.StopListening("stopFootSteps",stopFootSteps);
        _footStepsAudioSource.Stop();
        EventManager.StartListening("stopFootSteps",stopFootSteps);
    }
    
    public void wingsFlap()
    {
        EventManager.StopListening("wingsFlap", wingsFlap);
        _wingsFlappingAudioSource.clip = _wingsFlappingSound;
        if(!_wingsFlappingAudioSource.isPlaying)
            _wingsFlappingAudioSource.Play();
        EventManager.StartListening("wingsFlap",wingsFlap);
    }
    
    public void chickenDamage()
    {
        EventManager.StopListening("chickenDamage",chickenDamage);
        _chickenDamageAudioSource.clip = _chickenDamageSound;
        _chickenDamageAudioSource.Play();
        EventManager.StartListening("chickenDamage",chickenDamage);
    }
    
    public void chickenDeath()
    {
        EventManager.StopListening("chickenDeath",chickenDeath);
        _chickenDeathAudioSource.clip = _chickenDeathSound;
        if(!_chickenDeathAudioSource.isPlaying)
            _chickenDeathAudioSource.Play();
        EventManager.StartListening("chickenDeath",chickenDeath);
    }
    
}
