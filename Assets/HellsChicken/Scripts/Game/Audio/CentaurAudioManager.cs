using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class CentaurAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource centaurShotAudioSource;
    [SerializeField] private AudioSource QuestionMarkAudioSource;
    [SerializeField] private AudioSource centaurStepsAudioSource;
    [SerializeField] private AudioSource centaurDeathAudioSource;

    [SerializeField] private AudioClip centaurShotAudioSound;
    [SerializeField] private AudioClip QuestionMarkSound;
    [SerializeField] private AudioClip centaurStepsSound;
    [SerializeField] private AudioClip centaurDeathSound;


    protected void Awake() {
        EventManager.StartListening("centaurShot", centaurShot);
        EventManager.StartListening("centaurQuestionMark", CentaurQuestionMark);
        EventManager.StartListening("centaurSteps", CentaurSteps);
        EventManager.StartListening("centaurDeath", CentaurDeath);

    }


    public void centaurShot()
    {
        EventManager.StopListening("centaurShot", centaurShot);
        centaurShotAudioSource.clip = centaurShotAudioSound;
        centaurShotAudioSource.Play();
        EventManager.StartListening("centaurShot", centaurShot);
    }
    
    public void CentaurQuestionMark()
    {
        EventManager.StopListening("centaurQuestionMark", CentaurQuestionMark);
        QuestionMarkAudioSource.clip = QuestionMarkSound;
        if(!QuestionMarkAudioSource.isPlaying)
            QuestionMarkAudioSource.Play();
        EventManager.StartListening("centaurQuestionMark", CentaurQuestionMark);
    }
    
    public void CentaurSteps()
    {
        EventManager.StopListening("centaurSteps", CentaurSteps);
        centaurStepsAudioSource.clip = centaurStepsSound;
        centaurStepsAudioSource.Play();
        EventManager.StartListening("centaurSteps", CentaurSteps);
    }
    
    public void CentaurDeath()
    {
        EventManager.StopListening("centaurDeath", CentaurDeath);
        centaurDeathAudioSource.clip = centaurDeathSound; 
        centaurDeathAudioSource.Play();
        EventManager.StartListening("centaurDeath", CentaurDeath);
    }
}
