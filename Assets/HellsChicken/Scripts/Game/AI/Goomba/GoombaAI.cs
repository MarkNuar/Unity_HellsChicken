using System;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/*
 * AI of Goomba. Implemented as continue left - right movement. When he dies, he leaves a bomb that will explode in some seconds.
 */
public class GoombaAI : MonoBehaviour {
    
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject vanishEffectPrefab;
    [SerializeField] private bool right = true;
    
    private Rigidbody _rigidbody;
    private float agentVelocity = 8f;
    private bool isColliding = false;
    
    public Vector3 position, velocity;
    
    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (!isColliding) {
            if (right)
                _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.right);
            else
                _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.left);
            position = _rigidbody.position;
            velocity = _rigidbody.velocity;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            isColliding = true;
            Physics.IgnoreCollision(other.collider,GetComponent<CapsuleCollider>());
        }else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Enemy")) {
            right = !right;
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        }else if (other.gameObject.CompareTag("Attack")) {
            
            //creates the effect and starts the sound
            Instantiate(vanishEffectPrefab, transform.position, Quaternion.identity);
            EventManager.TriggerEvent("playTimerBomb");
            
            Destruction dest = GetComponent<Destruction>();
            if (dest != null)
            {
                dest.Destroyer();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Wall")) {
            right = !right;
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        }
    }

    void LateUpdate()
    {
        if (isColliding)
        {
            _rigidbody.position = position;
            _rigidbody.velocity = velocity;
        }
    }

    void OnCollisionExit(Collision collision){
        if (collision.collider.tag == "Player") 
            isColliding = false;
        
    }

}
