using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]

public class CentaurAI : MonoBehaviour {

    private int still = 0; //Variable for checking if I have to be still. 
    private bool movement = true; //Variable for checking if I can be still. 
    private bool right = true;
    
    [SerializeField] private float agentVelocity = 8f;
    [SerializeField] private float timeReaction = 2f; //how often do the agent take decision?
    [SerializeField] private Transform player;
    [SerializeField] private Transform arrowPosition;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject textPrefab;

    private Rigidbody _rigidbody;
    private DecisionTree tree;
    private GameObject textInstance;
    
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() {

        //Decision
        DTDecision d1 = new DTDecision(isPlayerVisible);
        DTDecision d2 = new DTDecision(isPlayerStill);
        
        //Action
        DTAction a1 = new DTAction(hit);
        DTAction a2 = new DTAction(move);
        DTAction a3 = new DTAction(stop);
        
        d1.addLink(true,a1);
        d1.addLink(false,d2);

        d2.addLink(true, a2);
        d2.addLink(false, a3);
        
        //root
        tree = new DecisionTree(d1);

        StartCoroutine(treeCoroutine());

    }

    // Update is called once per frame
    void Update(){
        
    }

    private void FixedUpdate() {
        if (movement) {
            if(right)
                _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.right);
            else
                _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.left);
        }
        else {
            _rigidbody.velocity = Vector3.zero;
        }
    }
    
    IEnumerator treeCoroutine() {
        while (true) {
            tree.start();
            yield return new WaitForSeconds(timeReaction);
        }
    }
    
    //Actions
    public object move() {
        movement = true;
        return null;
    }
    
    public object stop() {
        movement = false;
        return null;
    }

    public object hit() {
        GameObject arrow = Instantiate(arrowPrefab,arrowPosition.position,Quaternion.LookRotation(player.position,transform.position));
        Arrow ar = arrow.GetComponent<Arrow>();
        ar.Target = player;
        ar.Centaur = transform;
        ar.launch();
        return null;
    }

    //Decisions
    public object isPlayerVisible() {
        Vector3 ray = player.position - transform.position;
        RaycastHit hit;
        if ( ray.magnitude < 30 && Physics.Raycast(transform.position, ray, out hit)) {
            if (hit.transform == player) {
                //transform.LookAt(player.position);
                if (Vector3.Dot(ray, transform.forward) <= 0 ) {
                    transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                    right = !right;
                }
                
                if(textInstance != null)
                    Destroy(textInstance);
                
                movement = false;
                still = 0;
                return true;
            }
        }
        return false;
    }

    public object isPlayerStill() {
        if (still == 0) {
            if (Random.Range(0, 11) > 7) {
                still += 1;
                textInstance = Instantiate(textPrefab, gameObject.transform.position + new Vector3(-0.4f,3,0), Quaternion.identity);
                return false;
            }else 
                return true;
            
        }else {
            if (still > 0) {
                if (still == 2) {
                    still = -4;
                    Destroy(textInstance);
                    if (Random.Range(0, 2) == 0) {
                        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                        right = !right;
                    }
                }else
                    still += 1;
                return false;
            }
            else {
                still += 1;
                return true;
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("wall")) {
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            right = !right;
        }

        if (other.gameObject.CompareTag("Player")) {
            Physics.IgnoreCollision(other.collider, GetComponent<CapsuleCollider>());
        }
    }
}
