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
        Debug.Log("dio cane!");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("dio cane dio!");
            textTutorialTwo.gameObject.SetActive(false);
            textTutorialThree.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
