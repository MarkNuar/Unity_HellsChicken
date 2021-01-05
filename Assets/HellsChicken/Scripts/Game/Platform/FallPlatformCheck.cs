using System.Collections;
using System.Collections.Generic;
using HellsChicken.Scripts.Game.Platform;
using UnityEngine;

public class FallPlatformCheck : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            transform.parent.gameObject.GetComponent<FallenPlatformLevel2>().WaitForCollide1 = true;
        } 
    }
}
