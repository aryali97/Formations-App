﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowLinesToggleScript : MonoBehaviour {

    public Toggle toggle;

	// Use this for initialization
	void Start () {
        toggle.onValueChanged.AddListener(delegate {
            Toggled();
        });
	}
	
	// Update is called once per frame
	void Update () {
	}

    void Toggled() {
        bool beActive = toggle.isOn;
        foreach (Segment line in SegmentHelper.linesList) {
            line.gameObject.SetActive(beActive);
        }
        foreach (Endpoint point in SegmentHelper.pointsList) {
            point.gameObject.SetActive(beActive);
        }
    }
}
