using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TutorialOne : MonoBehaviour
{
    public GameObject textTutorialOne;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textTutorialOne.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
