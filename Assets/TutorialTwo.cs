using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTwo : MonoBehaviour
{
    public GameObject textTutorialOne;
    public GameObject textTutorialTwo;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {   
            textTutorialOne.gameObject.SetActive(false);
            textTutorialTwo.gameObject.SetActive(true);
        }
    }
}
