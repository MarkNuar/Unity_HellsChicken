using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

public class CentaurDissolvation : MonoBehaviour {

    private bool dissolvation = false;
    [SerializeField] private Renderer renderer;
    [SerializeField] private Material transparent;

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
        float i = 0.78f;
        yield return new WaitForSeconds(2f);
        renderer.material = transparent;
        while (i > 0) {
            renderer.material.SetColor("_BaseColor",new Color(1,1,1,i));
            i -= 0.03f;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject,1f);
    }
}