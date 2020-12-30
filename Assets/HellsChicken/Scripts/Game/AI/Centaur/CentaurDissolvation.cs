using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

public class CentaurDissolvation : MonoBehaviour {

    private bool dissolvation = false;
    [SerializeField] private Renderer renderer;

    public bool Dissolvation {
        get => dissolvation;
        set => dissolvation = value;
    }

    // Update is called once per frame
    void Update(){
        if (dissolvation) {
            
            StartCoroutine(dissolvingCentaur());
            dissolvation = false;
        }
    }

    IEnumerator dissolvingCentaur() {
        //print(renderer.material.GetInt("_SurfaceType"));
       // renderer.material.SetInt("_RenderQueueType", 4);
        float i = 0.97f;
        yield return new WaitForSeconds(2f);
        while (i > 0) {
            renderer.material.SetFloat("alpha",i);
            renderer.material.SetFloat("alpha2",i);
            i -= 0.03f;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject,1f);
    }
}