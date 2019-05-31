using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Endpoint : IDable {

    private Color hover_color_;
    private Color orig_color_;
    private Renderer renderer_;

    void Start() {
        renderer_ = GetComponent<Renderer>();
        hover_color_ = Color.magenta;
        orig_color_ = renderer_.material.color;
    }

    // Mouse hovers over
    void OnMouseEnter() {
        renderer_.material.color = hover_color_;
    }

    // Mouse leaves hover
    void OnMouseExit() {
        renderer_.material.color = orig_color_;
    }
	
	// Update is called once per frame
	void OnMouseDrag() {
        Endpoint end = GlobalVars.FindConnectedPoint(id_);
        if (!end) {
            return;
        }
        IDable line = GlobalVars.FindConnectedLine(id_);
        GlobalVars.UpdateLine(this.transform.position,
                              end.transform.position,
                              line.transform);
	}

    void OnMouseUp() {
        GlobalVars.UpdateLineRepr(GlobalVars.FindConnectedLine(id_));
    }
}
