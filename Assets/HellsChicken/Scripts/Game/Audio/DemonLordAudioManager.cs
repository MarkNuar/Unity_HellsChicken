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
    [SerializeField] private AudioSource demonLordStartFightAudioSource;
    [SerializeField] private AudioSource demonLord2HitComboAudioSource;
    [SerializeField] private AudioSource demonLord3HitComboAudioSource;



    [SerializeField] private AudioClip demonLordFootStepsSound;
    [SerializeField] private AudioClip demonLordWingsSound;
    [SerializeField] private AudioClip demonLordWhipSound;
    [SerializeField] private AudioClip demonLordDeathSound;
    [SerializeField] private AudioClip demonLordSwordSound;
    [SerializeField] private AudioClip demonLordRoarSound;
    [SerializeField] private AudioClip demonLordDamageSound;
    [SerializeField] private AudioClip demonLordStartFightSound;
    [SerializeField] private AudioClip demonLord2HitComboSound;
    [SerializeField] private AudioClip demonLord3HitComboSound;


    protected void Awake() {
        EventManager.StartListening("demonFootsteps", Footsteps);
        EventManager.StartListening("stopDemonFootsteps",StopFootsteps);
        EventManager.StartListening("demonWings", WingsFlap);
        EventManager.StartListening("demonDamage",DemonDamage);
        EventManager.StartListening("demonDeath",DemonDeath);
        EventManager.StartListening("demonWhip",DemonWhip);
        EventManager.StartListening("demonSword",DemonSword);
        EventManager.StartListening("demonRoar",DemonRoar);
        EventManager.StartListening("demonStartFight",DemonStartFight);
        EventManager.StartListening("demon2HitCombo",Demon2HitCombo);
        EventManager.StartListening("demon3HitCombo",Demon3HitCombo);

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
        EventManager.StopListening("stopDemonFootsteps", StopFootsteps);
        demonLordFootStepsAudioSource.Stop();
        EventManager.StartListening("stopDemonFootsteps", StopFootsteps);
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
    
    private void DemonStartFight()
    {
        EventManager.StopListening("demonStartFight",DemonStartFight);
        demonLordStartFightAudioSource.clip = demonLordStartFightSound;
        demonLordStartFightAudioSource.Play();
        EventManager.StartListening("demonStartFight",DemonStartFight);
    }
    
    private void Demon2HitCombo()
    {
        EventManager.StopListening("demon2HitCombo",Demon2HitCombo);
        demonLord2HitComboAudioSource.clip = demonLord2HitComboSound;
        demonLord2HitComboAudioSource.Play();
        EventManager.StartListening("demon2HitCombo",Demon2HitCombo);
    }
    
    private void Demon3HitCombo()
    {
        EventManager.StopListening("demon3HitCombo",Demon3HitCombo);
        demonLord3HitComboAudioSource.clip = demonLord3HitComboSound;
        demonLord3HitComboAudioSource.Play();
        EventManager.StartListening("demon3HitCombo",Demon3HitCombo);
    }
}
