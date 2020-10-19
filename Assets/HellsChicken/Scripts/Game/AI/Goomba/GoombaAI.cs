using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/*
 * AI of Goomba. Implemented as continue left - right movement. When he dies, he leaves a bomb that will explode in some seconds.
 */
public class GoombaAI : MonoBehaviour {
    
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject vanishEffectPrefab;

    private Rigidbody _rigidbody;
    private bool right = true;
    private float agentVelocity = 8f;
    
    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if(right)
            _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.right);
        else
            _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.left);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("wall")) {
            
            right = !right;
            
        }else if (other.gameObject.CompareTag("attack")) {
            
            //creates the effect and starts the sound
            Instantiate(vanishEffectPrefab, transform.position, Quaternion.identity);
            EventManager.TriggerEvent("playTimerBomb");
            
            //create the bomb the right new position
            Vector3 newPosition = new Vector3(gameObject.transform.position.x,1,gameObject.transform.position.z);
            Instantiate(bombPrefab,newPosition,Quaternion.Euler(0,0,90));
            
            //destroy the Goomba
            Destroy(gameObject);
            
        }
    }
}
