using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using EventManagerNamespace;
using UnityEngine;

public class LightningGeneratorScriptTwo : MonoBehaviour
{
    public LightningBoltScript lightningOne;
    public LightningBoltScript lightningTwo;
    public LightningBoltScript lightningThree;
    public LightningBoltScript lightningFour;
    public LightningBoltScript lightningFive;
    public LightningBoltScript lightningSix;
    public LightningBoltScript lightningSeven;
    public LightningBoltScript lightningEight;
    public LightningBoltScript lightningNine;
    public LightningBoltScript lightningTen;

    [SerializeField] public int lightningInterval = 5;
    public LightningBoltScript[] lightningBoltScripts;

    public AudioSource lightningAudioSource;
    public AudioClip lightningAudioClip;


    private void Awake()
    {
        EventManager.StartListening("lightningSound",lightningSound);
    }

    // Start is called before the first frame update
    void Start()
    {
        lightningBoltScripts = new LightningBoltScript[10];
        lightningBoltScripts[0] = lightningOne;
        lightningBoltScripts[1] = lightningTwo;
        lightningBoltScripts[2] = lightningThree;
        lightningBoltScripts[3] = lightningFour;
        lightningBoltScripts[4] = lightningFive;
        lightningBoltScripts[5] = lightningSix;
        lightningBoltScripts[6] = lightningSeven;
        lightningBoltScripts[7] = lightningEight;
        lightningBoltScripts[8] = lightningNine;
        lightningBoltScripts[9] = lightningTen;


        StartCoroutine(WaitForNextLightning(lightningInterval));

    }

    IEnumerator WaitForNextLightning(float timer)
    {
        while (true)
        {
            for (int i = 0; i < lightningBoltScripts.Length; i++)
            {
                yield return new WaitForSeconds(timer);
                lightningBoltScripts[i].Trigger();
                EventManager.TriggerEvent("lightningSound");
            }
        }
    }
    
    public void lightningSound()
    {
        EventManager.StopListening("lightningSound", lightningSound);
        lightningAudioSource.clip = lightningAudioClip;
        if(!lightningAudioSource.isPlaying)
            lightningAudioSource.Play();
        EventManager.StartListening("lightningSound", lightningSound);
    }
}
