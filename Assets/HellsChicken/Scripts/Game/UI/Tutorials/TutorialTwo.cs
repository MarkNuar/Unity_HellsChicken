using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTwo : MonoBehaviour
{
    public GameObject textTutorialOne;
    public GameObject textTutorialTwo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {   
            textTutorialOne.gameObject.SetActive(false);
            textTutorialTwo.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
