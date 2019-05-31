using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Endpoint : IDable {
	
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
