using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Endpoint : IDable {

    void Start() {
    }
	
	// Update is called once per frame
	void OnMouseDrag() {
        Endpoint end = SegmentHelper.FindConnectedPoint(id);
        if (!end) {
            return;
        }
        IDable line = SegmentHelper.FindConnectedLine(id);
        SegmentHelper.UpdateLine(this.transform.position,
                              end.transform.position,
                              line.transform);
	}

    void OnMouseUp() {
        SegmentHelper.UpdateLineRepr(SegmentHelper.FindConnectedLine(id));
    }
}
