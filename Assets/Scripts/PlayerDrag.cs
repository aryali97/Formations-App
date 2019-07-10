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

	// Use this for initialization
	void Start () {
	    snapToggle = GameObject.Find("Snap Toggle").GetComponent<Toggle>();	
        snapDist = 0.3f;
        centOffset = Vector3.negativeInfinity;
	}

    void OnMouseDown() {
        PlayerSelect.ClickHandler(gameObject);
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
        if (snapToggle.isOn) {
            Vector3 bestOffset = Vector3.negativeInfinity;
            foreach (IDable idable in GlobalVars.selected) {
                var adjPos = idable.transform.position + offset;
                var snappedPos = SegmentHelper.SnapToLines(adjPos, snapDist);
                var newOffset = snappedPos - idable.transform.position;
                if (newOffset.magnitude < bestOffset.magnitude) {
                    bestOffset = newOffset;
                }
            }
            offset = bestOffset;
        }
        foreach (IDable idable in GlobalVars.selected) {
            var selNewPos = idable.transform.position + offset;
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
