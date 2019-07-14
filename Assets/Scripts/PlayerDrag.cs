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

    void OnMouseDown() {
        var selected = GameObject.FindWithTag("Drawing Pane")
            .GetComponent<DrawingScript>().selected;
        if (selected != null) {
            selected.Unselect();
        }
    }

    Vector3 BorderClampOffset(Vector3 pos) {
        float xMinOffset = 0.0f;
        float xMaxOffset = 0.0f;
        float zMinOffset = 0.0f;
        float zMaxOffset = 0.0f;

        foreach (IDable idable in GlobalVars.selected) {
            xMinOffset = Math.Max(xMinOffset,
                (transform.position - idable.transform.position).x);
            xMaxOffset = Math.Max(xMaxOffset,
                (idable.transform.position - transform.position).x);
            zMinOffset = Math.Max(zMinOffset,
                (transform.position - idable.transform.position).z);
            zMaxOffset = Math.Max(zMaxOffset,
                (idable.transform.position - transform.position).z);
        }

        pos.x = Mathf.Clamp(pos.x, SegmentHelper.stageXMin + xMinOffset,
                                   SegmentHelper.stageXMax - xMaxOffset);
        pos.z = Mathf.Clamp(pos.z, SegmentHelper.stageZMin + zMinOffset,
                                   SegmentHelper.stageZMax - zMaxOffset);
        return (pos - transform.position);
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

        bool debugSelectSnap = false;
        if (snapToggle.isOn) {
            Vector3 bestOffset = Vector3.negativeInfinity;
            // Fake max
            int bestDimSnap = 0;
            if (GlobalVars.debugSelectSnapFlag && 
                Input.GetKey(KeyCode.Space) && 
                (lastSpace == null || Time.time - lastSpace > 0.5f)) {
                lastSpace = Time.time;
                debugSelectSnap = true;
            }
            foreach (IDable idable in GlobalVars.selected) {
                if (debugSelectSnap) {
                    Debug.Log("Debugging: " + idable.transform.name);
                }
                var adjPos = idable.transform.position + offset;
                int dimSnap;
                var snappedPos = SegmentHelper.SnapToLines(
                    adjPos,
                    snapDist,
                    out dimSnap);
                var newOffset = snappedPos - adjPos;
                if (debugSelectSnap) {
                    if (snappedPos == adjPos) {
                        Debug.Log("Offset is zero");
                    } else {
                        Debug.Log("Offset is NOT zero");
                        Debug.Log(newOffset);
                        Debug.Log(dimSnap);
                    }
                }
                float dimBreaker = 0.15f;
                if (snappedPos != adjPos) {
                    if (newOffset.magnitude < bestOffset.magnitude &&
                        ((bestOffset.magnitude - newOffset.magnitude) > dimBreaker ||
                         bestDimSnap <= dimSnap)) {
                        bestOffset = newOffset;
                        bestDimSnap = dimSnap;
                    } else if (Math.Abs(newOffset.magnitude - bestOffset.magnitude)
                        < dimBreaker && bestDimSnap < dimSnap) {
                        bestOffset = newOffset;
                        bestDimSnap = dimSnap;
                    }                 }
                if (debugSelectSnap) {
                    Debug.Log("Best offset for " + idable.transform.name +
                              ": " + bestOffset);
                }
            }
            if (bestOffset.x != float.NegativeInfinity) {
                snapOffset = bestOffset;
            }
            if (debugSelectSnap) {
                Debug.Log("Snap offset is: " + (snapOffset + offset));
            }
        }
        var finalOffset = BorderClampOffset(
            transform.position +  offset + snapOffset);
        foreach (IDable idable in GlobalVars.selected) {
            var selNewPos = idable.transform.position + finalOffset;
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

    void Update() {
        if (transform.position.y < -10.0f) {
            transform.position = new Vector3(0, 5.0f, 0);
            PlayerSelect.Unselect(gameObject);
        }
    }
}
