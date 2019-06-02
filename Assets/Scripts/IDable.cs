using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDable : MonoBehaviour {
    
    public int id;

    void Awake() {
        id = GlobalVars.nextId;
        GlobalVars.nextId++;
    }
}
