using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverable : MonoBehaviour {
    private Renderer renderer;
    private Drawable idable;

	// Use this for initialization
	void Start () {
	    renderer = GetComponent<Renderer>();
        idable = GetComponent<Drawable>();
	}
    
    // Mouse hovers
    public void OnMouseEnter() {
        if (idable.selected) {
            return;
        }
        renderer.material.color = GlobalVars.hoverColor;
    }

    // Mouse leaves hover
    public void OnMouseExit() {
        if (idable.selected) {
            return;
        }
        renderer.material.color = GlobalVars.defaultColor;
    }
}
