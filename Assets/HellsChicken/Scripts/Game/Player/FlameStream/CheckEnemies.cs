﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemies : MonoBehaviour {
    
    private List<String> _goList;

    public List<string> GOList {
        get => _goList;
        set => _goList = value;
    }

    private void Start() {
        _goList = new List<string>();
    }
    
}