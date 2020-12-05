using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class DeathGoombaAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bombTimerAudioSource;
    [SerializeField] private AudioSource GoombaExplosionAudioSource;

    [SerializeField] private AudioClip bombTimerSound;
    [SerializeField] private AudioClip GoombaExplosionSound;



    private void Awake()
    {
        EventManager.StartListening("playTimerBomb",PlayTimerBomb);
        EventManager.StartListening("GoombaExplosion",GoombaExplosion);
    }

    private void PlayTimerBomb()
    {
        EventManager.StopListening("playTimerBomb",PlayTimerBomb);
        bombTimerAudioSource.clip = bombTimerSound;
        bombTimerAudioSource.Play();
        EventManager.StartListening("playTimerBomb",PlayTimerBomb);
    }

    private void GoombaExplosion()
    {
        EventManager.StopListening("GoombaExplosion",GoombaExplosion);
        GoombaExplosionAudioSource.clip = GoombaExplosionSound;
        GoombaExplosionAudioSource.Play();
        EventManager.StartListening("GoombaExplosion",GoombaExplosion);
    }
}
