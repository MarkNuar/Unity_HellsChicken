using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource gameAudioSource;

    [SerializeField] private AudioClip gameSoundtrackSound;

    private void Awake()
    {
        EventManager.StartListening("gameSoundtrack",gameSoundtrack);
    }

    private void gameSoundtrack()
    {
        EventManager.StopListening("gameSoundtrack",gameSoundtrack);
        gameAudioSource.clip = gameSoundtrackSound;
        if(!gameAudioSource.isPlaying)
            gameAudioSource.Play();
        EventManager.StartListening("gameSoundtrack",gameSoundtrack);
    }

    private void Start()
    {
        EventManager.TriggerEvent("gameSoundtrack");
    }
}
