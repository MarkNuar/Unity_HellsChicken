using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EventManagerNamespace;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;
    
    [SerializeField] private AudioSource gameAudioSource;

    [SerializeField] private AudioClip gameSoundtrackSound;

    private void Awake()
    {

        if (Instance == null)
        {
            EventManager.StartListening("gameSoundtrack",gameSoundtrack);
            EventManager.TriggerEvent("gameSoundtrack");
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void gameSoundtrack()
    {
        EventManager.StopListening("gameSoundtrack",gameSoundtrack);
        gameAudioSource.clip = gameSoundtrackSound;
        gameAudioSource.loop = true;
        gameAudioSource.Play();
        EventManager.StartListening("gameSoundtrack",gameSoundtrack);
    }
    
}
