using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CentaurFire : MonoBehaviour {
    
    [SerializeField] private GameObject contactExplosion;
    [SerializeField] private float initialVelocity = 20f;
    [SerializeField] private Animator _bowAnimator;
    
    private Rigidbody _rigidbody;
    private Vector3 target;
    private Vector3 centaurPos;
    private Vector3 lastPos;
    
    public Vector3 Target {
        set => target = value;
    }
    
    public Vector3 CentaurPos {
        set => centaurPos = value;
    }
    
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        transform.rotation = LookAt2D(_rigidbody.velocity);
    }
    
    //Search for the angle for the trajectory.
    public void findAngle(bool right) {
         float angleCentaurTarget;
         Vector3 rotatedVector;
         
         float g = -Physics.gravity.y;
         float v2 = initialVelocity * initialVelocity;
         float v4 = v2 * v2;
         float x = Mathf.Abs((target.x - centaurPos.x));
         float y = Mathf.Abs((target.y - centaurPos.y));
         float x2 = x * x;
         float squareRoot = (float)Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2));
        
         //This is the angle to be applied from the horizontal vector (but i couldn't get it).
         float angle = Mathf.Atan((v2 - squareRoot) / (g * x))*Mathf.Rad2Deg;

         //Different angle if i am going right or left.
         if (!right) { //Calculate the angle beetween the horizontal vector (here works) and the vector from the centaur to the target.
            angleCentaurTarget = Mathf.Acos(Vector3.Dot(Vector3.left, (target - centaurPos).normalized)) *
                      Mathf.Rad2Deg;
            //Substracted the 2 angle and apply the rotation from the target (not from horizontal).
            rotatedVector = Quaternion.AngleAxis(angle - angleCentaurTarget, Vector3.back) *
                            (target - centaurPos); 
         }else {
             angleCentaurTarget = Mathf.Acos(Vector3.Dot(Vector3.left, (target - centaurPos).normalized)) *
                            Mathf.Rad2Deg;
             angleCentaurTarget = 180 - angleCentaurTarget;
             rotatedVector = Quaternion.AngleAxis(angle - angleCentaurTarget, Vector3.forward) *
                                  (target - centaurPos); 
         }

         rotatedVector += centaurPos;

         //Apply new velocity
         _rigidbody.velocity = (rotatedVector - centaurPos).normalized * initialVelocity;
    }

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Physics.IgnoreCollision(other.collider, GetComponent<BoxCollider>());
        }else if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall")
          || other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Lava")) {
            Destroy(gameObject);
        }
        if(!other.gameObject.CompareTag("Enemy"))
            Instantiate(contactExplosion, other.contacts[0].point, Quaternion.identity);
    }
    
    static Quaternion LookAt2D(Vector2 forward) {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
}
