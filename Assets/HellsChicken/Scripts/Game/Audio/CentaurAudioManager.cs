using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class CentaurAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource centaurShotAudioSource;
    [SerializeField] private AudioSource QuestionMarkAudioSource;
    [SerializeField] private AudioSource centaurStepsAudioSource;
    [SerializeField] private AudioSource playerVisibleAudioSource;

    [SerializeField] private AudioClip centaurShotAudioSound;
    [SerializeField] private AudioClip QuestionMarkSound;
    [SerializeField] private AudioClip centaurStepsSound;
    [SerializeField] private AudioClip playerVisibleSound;


    protected void Awake() {
        EventManager.StartListening("centaurShot", centaurShot);
        EventManager.StartListening("centaurQuestionMark", CentaurQuestionMark);
        EventManager.StartListening("centaurSteps", CentaurSteps);
        EventManager.StartListening("playerVisible", PlayerVisible);

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
    
    public void PlayerVisible()
    {
        EventManager.StopListening("playerVisible", PlayerVisible);
        playerVisibleAudioSource.clip = playerVisibleSound;
        playerVisibleAudioSource.Play();
        EventManager.StartListening("playerVisible", PlayerVisible);
    }
}
