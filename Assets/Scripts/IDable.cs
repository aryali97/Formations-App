﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDable : MonoBehaviour {
    
    public int id;

    private static int nextId = 0;

    void Awake() {
        id = nextId;
        nextId++;
    }
}
