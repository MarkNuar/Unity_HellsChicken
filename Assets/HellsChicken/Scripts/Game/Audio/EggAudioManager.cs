using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Audio
{
    public class EggAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource bombAudioSource;
        [SerializeField] private AudioSource bombTimerAudioSource;
    
        [SerializeField] private AudioClip bombSound;
        [SerializeField] private AudioClip bombTimerSound;

        private void Awake()
        {
            EventManager.StartListening("playBomb",PlayBomb);
            EventManager.StartListening("playTimerBomb",PlayTimerBomb);
        }
    
        private void OnDisable()
        {
            EventManager.StopListening("playBomb",PlayBomb);
            EventManager.StopListening("playTimerBomb",PlayTimerBomb);
        }

        private void PlayBomb()
        {
            EventManager.StartListening("playBomb",PlayBomb);
            bombAudioSource.clip = bombSound;
            bombAudioSource.Play();
            EventManager.StopListening("playBomb",PlayBomb);
        }
    
        private void PlayTimerBomb()
        {
            EventManager.StopListening("playTimerBomb",PlayTimerBomb);
            bombTimerAudioSource.clip = bombTimerSound;
            bombTimerAudioSource.time = 1f;
            bombTimerAudioSource.Play();
            EventManager.StartListening("playTimerBomb",PlayTimerBomb);
        }
    }
}
