using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Audio
{
    public class IntroAudioManager : MonoBehaviour
    {
        public static IntroAudioManager Instance;
    
        [SerializeField] private AudioSource introAudioSource;

        [SerializeField] private AudioClip introSoundtrackSound;

        [SerializeField] private AudioSource voiceOverAudioSource;
        
        [SerializeField] private List<AudioClip> voiceOvers;
            
        
        private void Awake()
        {
            if (Instance == null)
            {
                EventManager.StartListening("startIntroSoundtrack",StartIntroSoundtrack);
                EventManager.StartListening("stopIntroSoundtrack",StopIntroSoundtrack);
                EventManager.StartListening("startVoiceOver",StartVoiceOver);
                EventManager.StartListening("stopVoiceOver",StopVoiceOver);
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            
            else
            {
                Destroy(gameObject);
            }
        }

        private void StartIntroSoundtrack()
        {
            EventManager.StopListening("startIntroSoundtrack",StartIntroSoundtrack);
            introAudioSource.clip = introSoundtrackSound;
            introAudioSource.loop = true;
            introAudioSource.Play();
            EventManager.StartListening("startIntroSoundtrack",StartIntroSoundtrack);
        }

        private void StopIntroSoundtrack()
        {
            EventManager.StopListening("stopIntroSoundtrack",StopIntroSoundtrack);
            StartCoroutine(StartFade(introAudioSource, 1, 0));
            DestroyGameAudioManagerInstance(1);
            EventManager.StartListening("stopIntroSoundtrack",StopIntroSoundtrack);
        }

        private void StartVoiceOver(int voiceOverIndex)
        {
            EventManager.StopListening("startVoiceOver",StartVoiceOver);
            voiceOverAudioSource.clip = voiceOvers[voiceOverIndex];
            voiceOverAudioSource.Play();
            EventManager.StartListening("startVoiceOver",StartVoiceOver);
        }

        private void StopVoiceOver()
        {
            EventManager.StopListening("stopVoiceOver",StopVoiceOver);
            StartCoroutine(StartFade(voiceOverAudioSource, 1, 0));
            EventManager.StartListening("stopVoiceOver",StopVoiceOver);
        }
        
        private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            audioSource.Stop();
        }
    
        private void DestroyGameAudioManagerInstance(float delay)
        {
            Destroy(gameObject,delay);
        }

        public float GetVoiceOverDuration(int index)
        {
            return voiceOvers[index].length;
        }
    }
}
