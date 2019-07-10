using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDrag : MonoBehaviour {

    public float y;

    private float snapDist;
    private Toggle snapToggle;
    private Vector3 centOffset;

    private float lastSpace;

	// Use this for initialization
	void Start () {
	    snapToggle = GameObject.Find("Snap Toggle").GetComponent<Toggle>();	
        snapDist = 0.4f;
        centOffset = Vector3.negativeInfinity;
	}

    void OnMouseDrag() {
        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (!plane.Raycast(ray, out distance)) {
            return;
        }

        // Set up center offset
        if (centOffset.x == float.NegativeInfinity) {
            centOffset = transform.position - ray.GetPoint(distance);
            centOffset.y = 0;
            return;
        }
        // Not a drag unless actually moving
        if ((centOffset.x + ray.GetPoint(distance).x == transform.position.x) &&
            (centOffset.z + ray.GetPoint(distance).z == transform.position.z)) {
            return;
        }

        var newPos = ray.GetPoint(distance) + centOffset;
        var offset = newPos - transform.position;
        offset.y = 0;
        Vector3 snapOffset = Vector3.zero;

        GlobalVars.debugSelectSnap = false;
        if (snapToggle.isOn) {
            Vector3 bestOffset = Vector3.negativeInfinity;
            if (Input.GetKey(KeyCode.Space) && (lastSpace == null || Time.time - lastSpace > 0.5f)) {
                lastSpace = Time.time;
                GlobalVars.debugSelectSnap = true;
            }
            foreach (IDable idable in GlobalVars.selected) {
                if (GlobalVars.debugSelectSnap) {
                    Debug.Log("Debugging: " + idable.transform.name);
                }
                var adjPos = idable.transform.position + offset;
                var snappedPos = SegmentHelper.SnapToLines(adjPos, snapDist);
                var newOffset = snappedPos - adjPos;
                if (GlobalVars.debugSelectSnap) {
                    if (snappedPos == adjPos) {
                        Debug.Log("Offset is zero");
                    } else {
                        Debug.Log("Offset is NOT zero");
                        Debug.Log(newOffset);
                    }
                }
                Debug.Log(bestOffset.magnitude);
                if (snappedPos != adjPos && newOffset.magnitude < bestOffset.magnitude) {
                    bestOffset = newOffset;
                } 
                if (GlobalVars.debugSelectSnap) {
                    Debug.Log("Best offset for " + idable.transform.name +
                              ": " + bestOffset);
                }
            }
            if (bestOffset.x != float.NegativeInfinity) {
                snapOffset = bestOffset;
            }
            if (GlobalVars.debugSelectSnap) {
                Debug.Log("Snap offset is: " + (snapOffset + offset));
            }
        }
        foreach (IDable idable in GlobalVars.selected) {
            var selNewPos = idable.transform.position + offset + snapOffset;
            selNewPos.y = y;
            idable.transform.position = selNewPos;
            var rb = idable.GetComponent<Rigidbody>();
            if (!rb.Equals(null)) {
                rb.velocity = Vector3.zero;
            }
        }
    }

    void OnMouseUp() {
        centOffset = Vector3.negativeInfinity;
    }
}
