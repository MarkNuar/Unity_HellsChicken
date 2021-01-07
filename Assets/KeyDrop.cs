using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class KeyDrop : MonoBehaviour
{
    private Rigidbody rb;
    public Transform _landingPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        EventManager.StartListening("keyDrop",triggerKeyDrop);
    }

    public void triggerKeyDrop()
    {
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (transform.position.y <= _landingPosition.position.y)
            rb.isKinematic = true;
    }
}
