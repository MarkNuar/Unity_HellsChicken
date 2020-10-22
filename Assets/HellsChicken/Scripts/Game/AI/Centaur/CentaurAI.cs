using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]

public class CentaurAI : MonoBehaviour {

    private bool still = true; //Variable for checking if I have to be still. 
    private bool movement = true; //Variable for checking if I can be still. 
    private bool right = true;
    
    [SerializeField] private float agentVelocity = 8f;
    [SerializeField] private float timeReaction = 2f; //how often do the agent take decision?
    
    private Rigidbody _rigidbody;
    private DecisionTree tree;
    
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() {
        
        //Decision
        DTDecision d1 = new DTDecision(isPlayerVisible);
        DTDecision d2 = new DTDecision(isPlayerStill);
        
        //Action
        DTAction a2 = new DTAction(move);
        DTAction a3 = new DTAction(stop);
        
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

    //Decisions
    public object isPlayerVisible() {
        return false;
    }

    public object isPlayerStill() {
        /*if (!still)
            return true;
        else {
            if (!movement)
                return false;
            else {*/
                int rnd = Random.Range(0, 11);
                if (rnd > 7)
                    return false;
                else
                    return true;
            /*}
        }*/
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("wall")) {
            right = !right;
        }
    }
}
