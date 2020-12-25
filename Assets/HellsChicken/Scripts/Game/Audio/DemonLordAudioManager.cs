using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class DemonLordAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource demonLordFootStepsAudioSource;
    [SerializeField] private AudioSource demonLordWingsAudioSource;
    [SerializeField] private AudioSource demonLordWhipAudioSource;
    [SerializeField] private AudioSource demonLordDeathAudioSource;
    [SerializeField] private AudioSource demonLordSwordAudioSource;
    [SerializeField] private AudioSource demonLordRoarAudioSource;
    [SerializeField] private AudioSource demonLordDamageAudioSource;

    [SerializeField] private AudioClip demonLordFootStepsSound;
    [SerializeField] private AudioClip demonLordWingsSound;
    [SerializeField] private AudioClip demonLordWhipSound;
    [SerializeField] private AudioClip demonLordDeathSound;
    [SerializeField] private AudioClip demonLordSwordSound;
    [SerializeField] private AudioClip demonLordRoarSound;
    [SerializeField] private AudioClip demonLordDamageSound;


    protected void Awake() {
        EventManager.StartListening("demonFootsteps", Footsteps);
        EventManager.StartListening("stopDemonFootsteps",StopFootsteps);
        EventManager.StartListening("demonWings", WingsFlap);
        EventManager.StartListening("demonDamage",DemonDamage);
        EventManager.StartListening("demonDeath",DemonDeath);
        EventManager.StartListening("demonWhip",DemonWhip);
        EventManager.StartListening("demonSword",DemonSword);
        EventManager.StartListening("demonRoar",DemonRoar);

    }

    private void Footsteps()
    {
        EventManager.StopListening("demonFootsteps", Footsteps);
        demonLordFootStepsAudioSource.clip = demonLordFootStepsSound;
        if(!demonLordFootStepsAudioSource.isPlaying)
            demonLordFootStepsAudioSource.Play();
        EventManager.StartListening("demonFootsteps", Footsteps);
    }
    
    private void StopFootsteps()
    {
        EventManager.StopListening("demonFootsteps", StopFootsteps);
        demonLordFootStepsAudioSource.Stop();
        EventManager.StartListening("demonFootsteps", StopFootsteps);
    }
    
    private void WingsFlap()
    {
        EventManager.StopListening("wingsFlap", WingsFlap);
        demonLordWingsAudioSource.clip = demonLordWingsSound;
        if(!demonLordWingsAudioSource.isPlaying)
            demonLordWingsAudioSource.Play();
        EventManager.StartListening("wingsFlap", WingsFlap);
    }

    private void DemonDamage()
    {
        EventManager.StopListening("demonDamage",DemonDamage);
        demonLordDamageAudioSource.clip = demonLordDamageSound;
        demonLordDamageAudioSource.Play();
        EventManager.StartListening("demonDamage",DemonDamage);
    }
    
    private void DemonDeath()
    {
        EventManager.StopListening("demonDeath",DemonDeath);
        demonLordDeathAudioSource.clip = demonLordDeathSound;
        demonLordDeathAudioSource.Play();
        EventManager.StartListening("demonDeath",DemonDeath);
    }
    
    private void DemonWhip()
    {
        EventManager.StopListening("demonWhip",DemonWhip);
        demonLordWhipAudioSource.clip = demonLordWhipSound;
        demonLordWhipAudioSource.Play();
        EventManager.StartListening("demonWhip",DemonWhip);
    }
    
    private void DemonSword()
    {
        EventManager.StopListening("demonSword",DemonSword);
        demonLordSwordAudioSource.clip = demonLordSwordSound;
        demonLordSwordAudioSource.Play();
        EventManager.StartListening("demonSword",DemonSword);
    }
    
    private void DemonRoar()
    {
        EventManager.StopListening("demonRoar",DemonRoar);
        demonLordRoarAudioSource.clip = demonLordRoarSound;
        demonLordRoarAudioSource.Play();
        EventManager.StartListening("demonRoar",DemonRoar);
    }
}
