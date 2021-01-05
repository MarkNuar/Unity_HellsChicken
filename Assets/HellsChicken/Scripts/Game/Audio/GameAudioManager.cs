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
            EventManager.StartListening("stopGameSoundtrack",StopGameSoundtrack);
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

    private void StopGameSoundtrack()
    {
        EventManager.StopListening("stopGameSoundtrack",StopGameSoundtrack);
        StartCoroutine(StartFade(gameAudioSource, 0.95f, 0));
        EventManager.StartListening("stopGameSoundtrack",StopGameSoundtrack);
    }
    
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
    }
    
    public void DestroyGameAudioManagerInstance(float time)
    {
        Destroy(gameObject,time);
    }
    
}
