using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using Unity.Mathematics;
using UnityEngine;

public class BowDirection : MonoBehaviour {
    
    [SerializeField] private GameObject centaur;

    private void LateUpdate() {
        transform.position = new Vector3(centaur.transform.position.x,centaur.transform.position.y,transform.position.z);
    }

    // Start is called before the first frame update
    void Start(){
        EventManager.StartListening("changeBowDirection",changeDirection);
    }

    void changeDirection() {
        EventManager.StopListening("changeBowDirection",changeDirection);
        transform.rotation = Quaternion.Euler(0,180,0) * transform.rotation;
        EventManager.StartListening("changeBowDirection",changeDirection);
    }
    
}
