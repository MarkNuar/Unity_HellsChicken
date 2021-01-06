using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class BossMusicTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        EventManager.TriggerEvent("stopGameSoundtrack");
        EventManager.TriggerEvent("startBossMusic");
    }
    
}
