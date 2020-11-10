using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;

public class ParticleCollision : MonoBehaviour
{
    public ParticleSystem part;
    
    private List<ParticleCollisionEvent> collisionEvents;

    public void Awake() {
        part = GetComponent<ParticleSystem>();
    }

    void Start(){
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Destruction destr = other.GetComponent<Destruction>();
            if(destr == null)    
                destr = other.gameObject.transform.parent.GetComponent<Destruction>();
            destr.Destroyer();
        }
    }
}