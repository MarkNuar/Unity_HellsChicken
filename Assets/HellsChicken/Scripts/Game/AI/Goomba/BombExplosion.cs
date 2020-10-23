using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{

    [SerializeField] private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(makeExplosion());
    }
    
    //Wait for 2 second and then make the bomb explode.
    IEnumerator makeExplosion() {  
        yield return new WaitForSeconds(2);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            EventManager.TriggerEvent("playBomb");
            Destroy(gameObject);
        yield return null;
    }
    
}
