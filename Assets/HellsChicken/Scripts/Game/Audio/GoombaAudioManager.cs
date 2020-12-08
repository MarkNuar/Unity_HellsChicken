using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagerNamespace;


public class GoombaAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource goombaFootstepsAudioSource;

    [SerializeField] private AudioClip goombaFootstepsSound;
    
    protected void Awake() {
        EventManager.StartListening("goombaFootsteps", goombaFootsteps);
    }


    public void goombaFootsteps()
    {
        EventManager.StopListening("goombaFootsteps", goombaFootsteps);
        goombaFootstepsAudioSource.clip = goombaFootstepsSound;
        if(!goombaFootstepsAudioSource.isPlaying)
            goombaFootstepsAudioSource.Play();
        EventManager.StartListening("goombaFootsteps", goombaFootsteps);
    }
}
