using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawable : IDable 
{

    public bool selected;
    private DrawingScript controller;

    void Awake() 
    {
        base.Awake();
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
        selected = true;
    }

    public void Unselect() {
        GetComponent<Renderer>().material.color = GlobalVars.defaultColor;
        selected = false;
    }
}
