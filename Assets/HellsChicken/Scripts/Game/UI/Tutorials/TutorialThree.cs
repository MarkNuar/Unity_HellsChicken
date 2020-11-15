using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialThree : MonoBehaviour
{
    public GameObject textTutorialTwo;
    public GameObject textTutorialThree;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textTutorialTwo.gameObject.SetActive(false);
            textTutorialThree.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
