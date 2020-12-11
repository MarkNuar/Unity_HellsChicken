using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class MenuAudioSource : MonoBehaviour
{
    [SerializeField] private AudioSource menuAudioSource;

    [SerializeField] private AudioClip menuSoundtrackSound;
    
    protected void Awake() {
        EventManager.StartListening("menuSoundtrack", menuSoundtrack);
        EventManager.StartListening("fadeOutMusic", fadeOutMenuSoundtrack);
    }


    public void menuSoundtrack()
    {
        EventManager.StopListening("menuSoundtrack", menuSoundtrack);
        menuAudioSource.clip = menuSoundtrackSound;
        menuAudioSource.Play();
        EventManager.StartListening("menuSoundtrack", menuSoundtrack);
    }

    public void fadeOutMenuSoundtrack()
    {
        EventManager.StopListening("fadeOutMusic", fadeOutMenuSoundtrack);
        StartCoroutine(StartFade(menuAudioSource, 1, 0));
        EventManager.StartListening("fadeOutMusic", fadeOutMenuSoundtrack);
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
}
