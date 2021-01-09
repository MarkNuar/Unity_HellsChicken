using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Audio
{
    public class GameAudioManager : MonoBehaviour
    {
        public static GameAudioManager Instance;
    
        [SerializeField] private AudioSource gameAudioSource;
        [SerializeField] private AudioSource bossAudioSource;

        [SerializeField] private AudioClip gameSoundtrack;
        [SerializeField] private AudioClip bossSoundtrack;

        private List<AudioSource> _audioSources;
        
        private void Awake()
        {

            if (Instance == null)
            {
                _audioSources = new List<AudioSource> {gameAudioSource, bossAudioSource};
                Debug.Log("audio sources: "+_audioSources.Count);
                
                EventManager.StartListening("startGameSoundtrack",StartGameSoundtrack);
                EventManager.StartListening("stopGameSoundtrack",StopGameSoundtrack);
            
                EventManager.StartListening("startBossMusic",StartBossMusic);
                EventManager.StartListening("stopBossMusic",StopBossMusic);
                
                EventManager.StartListening("stopAllMusics",StopAllMusics);
            
                EventManager.TriggerEvent("startGameSoundtrack");
            
                Instance = this;
                DontDestroyOnLoad(Instance);
            }

            else
            {
                Destroy(gameObject);
            }
        }

        private void StartGameSoundtrack()
        {
            EventManager.StopListening("startGameSoundtrack",StartGameSoundtrack);
            gameAudioSource.clip = gameSoundtrack;
            gameAudioSource.loop = true;
            gameAudioSource.Play();
            EventManager.StartListening("startGameSoundtrack",StartGameSoundtrack);
        }

        private void StopGameSoundtrack()
        {
            EventManager.StopListening("stopGameSoundtrack",StopGameSoundtrack);
            StartCoroutine(StartFade(gameAudioSource, 0.95f, 0));
            EventManager.StartListening("stopGameSoundtrack",StopGameSoundtrack);
        }
    
        private void StartBossMusic()
        {
            EventManager.StopListening("startBossMusic",StartBossMusic);
            bossAudioSource.clip = bossSoundtrack;
            bossAudioSource.loop = true;
            if(!bossAudioSource.isPlaying)
                bossAudioSource.Play();
            StartCoroutine(StartFade(bossAudioSource, 2f, 1));
            EventManager.StartListening("startBossMusic",StartBossMusic);
        }

        private void StopBossMusic()
        {
            EventManager.StopListening("stopGameSoundtrack",StopBossMusic);
            StartCoroutine(StartFade(bossAudioSource, 0.95f, 0));
            EventManager.StartListening("stopGameSoundtrack",StopBossMusic);
        }

        private void StopAllMusics()
        {
            EventManager.StopListening("stopAllMusics",StopAllMusics);
            foreach (var source in _audioSources)
            {
                StartCoroutine(StartFade(source, 0.95f, 0));
            }
            EventManager.StartListening("stopAllMusics",StopAllMusics);
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
}
