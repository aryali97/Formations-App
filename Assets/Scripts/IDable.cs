using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDable : MonoBehaviour {
    
    private static int nextId = 1;

    public int id;
    public bool selected;
    private DrawingScript controller;

    void Awake() {
        id = nextId;
        nextId++;
        controller = GameObject.FindWithTag("Drawing Pane").GetComponent<DrawingScript>();
        selected = false;
    }

    void OnMouseDown() {
        Select();
    }

    public void Select() {
        if (controller.selected) {
            controller.selected.Unselect();
        }
        controller.selected = this;
        GetComponent<Renderer>().material.color = GlobalVars.activeColor;
        Debug.Log("Color set");
        Debug.Log(GetComponent<Renderer>().material.color);
        selected = true;
    }

    public void Unselect() {
        GetComponent<Renderer>().material.color = GlobalVars.defaultColor;
        selected = false;
    }
}
