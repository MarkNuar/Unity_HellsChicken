using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour {

    [SerializeField] private GameObject explosionPrefab;

    private GameObject explosion;
    private ParticleSystem _particleSystem;

  

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(makeExplosion());
    }

    // Update is called once per frame
    void Update(){
        
        /*if (explosion != null && !_particleSystem.isPlaying) {
            Destroy(explosion);
        }*/
        


    }

    IEnumerator makeExplosion() {
        yield return new WaitForSeconds(2);
            explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            _particleSystem = explosion.GetComponent<ParticleSystem>();
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            Destroy(gameObject);
        yield return null;
    }
    
}
