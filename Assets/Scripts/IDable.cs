using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDable : MonoBehaviour {
    
    private static int nextId = 1;

    public int id;

    protected void Awake() {
        id = nextId;
        nextId++;
    }
}
