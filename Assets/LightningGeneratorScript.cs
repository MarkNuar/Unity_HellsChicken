using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class LightningGeneratorScript : MonoBehaviour
{
    public LightningBoltScript lightningOne;
    public LightningBoltScript lightningTwo;
    public LightningBoltScript lightningThree;
    public LightningBoltScript lightningFour;
    public LightningBoltScript lightningFive;
    public LightningBoltScript lightningSix;

    public LightningBoltScript[] lightningBoltScripts;
    // Start is called before the first frame update
    void Start()
    {
        lightningBoltScripts = new LightningBoltScript[6];
        lightningBoltScripts[0] = lightningOne;
        lightningBoltScripts[1] = lightningTwo;
        lightningBoltScripts[2] = lightningThree;
        lightningBoltScripts[3] = lightningFour;
        lightningBoltScripts[4] = lightningFive;
        lightningBoltScripts[5] = lightningSix;

            for (int i = 0; i < lightningBoltScripts.Length; i++)
                StartCoroutine(WaitForNextLightning(5f,i));

    }

    IEnumerator WaitForNextLightning(float time, int i)
    {
        yield return new WaitForSeconds(time);
        lightningBoltScripts[i].Trigger();
    }
}
