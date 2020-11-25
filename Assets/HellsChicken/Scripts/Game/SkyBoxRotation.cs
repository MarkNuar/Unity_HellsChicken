using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRotation : MonoBehaviour {

    private Material skyBoxMaterial;
    private float i = 0;
    
    private void Awake() {
        skyBoxMaterial = RenderSettings.skybox;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        i = (i + 0.03f) % 360;
        skyBoxMaterial.SetFloat("_Rotation",i);
    }
}
