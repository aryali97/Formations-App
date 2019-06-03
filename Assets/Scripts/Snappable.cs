using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snappable : MonoBehaviour {

    public float y;

    private float snapDist;
    private Toggle snapToggle;

	// Use this for initialization
	void Start () {
	    snapToggle = GameObject.Find("Snap Toggle").GetComponent<Toggle>();	
        snapDist = 0.3f;
	}

    void OnMouseDown() {
    }

    void OnMouseDrag() {
        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance)) {
            var newPos = ray.GetPoint(distance);
            if (snapToggle.isOn) {
                var thisEp = GetComponent<Endpoint>();
                HashSet<int> omitIds = thisEp ?
                    thisEp.connectSegmentIdSet() :
                    new HashSet<int>();
                var snappedCoords = SegmentHelper.SnapToLines(
                    newPos,
                    snapDist,
                    omitIds);
                transform.position = new Vector3(
                    snappedCoords.x,
                    y,
                    snappedCoords.z);
            } else {
                transform.position = new Vector3(newPos.x, y, newPos.z);
            }
            var rb = GetComponent<Rigidbody>();
            if (!rb.Equals(null)) {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
