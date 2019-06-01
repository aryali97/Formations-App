using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverable : MonoBehaviour {

    private Color hoverColor;
    private Color origColor;
    private Renderer renderer;

	// Use this for initialization
	void Start () {
	    renderer = GetComponent<Renderer>();
        hoverColor = Color.magenta;
        origColor = renderer.material.color;
	}
    
    // Mouse hovers
    public void OnMouseEnter() {
        renderer.material.color = hoverColor;
    }

    // Mouse leaves hover
    public void OnMouseExit() {
        renderer.material.color = origColor;
    }
}
