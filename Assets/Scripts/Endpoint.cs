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
        //Debug.Log("OWN ID IS " + id_);
        /*
        if (!GlobalVars.pointToPoint.ContainsKey(id) ||
            !GlobalVars.pointToLine.ContainsKey(id)) {
            return;
        }
        Debug.Log("Endpoint ID found");
        Vector3 end = transform.position;
        Vector3 start = ((GameObject)EditorUtility.InstanceIDToObject(
            GlobalVars.pointToPoint[id])).transform.position;
        float dist = (float)Math.Sqrt(
            Math.Pow(start.x - end.x, 2) +
            Math.Pow(start.z - end.z, 2));
        float angle = Mathf.Atan2(
            end.x - start.x,
            end.z - start.z) * Mathf.Rad2Deg;
        Transform planeT = (GameObject.Find(id.ToString())).transform;
        planeT.position = (end + start)/2.0f;
        planeT.rotation = Quaternion.Euler(0, angle, 0);
        planeT.localScale = new Vector3(
            planeT.localScale.x,
            planeT.localScale.y,
            dist/10.0f);
        */
	}

    void OnMouseUp() {
        GlobalVars.UpdateLineRepr(GlobalVars.FindConnectedLine(id_));
    }
}
