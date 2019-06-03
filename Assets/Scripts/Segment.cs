using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : IDable {

    public Tuple<Endpoint, Endpoint> points;
    private float y;
    private float snapDist;
    private Vector3 centOffset;

	// Use this for initialization
	void Start () {
        y = 0.6f;
        snapDist = 0.3f;
	}
    void OnMouseDown() {
        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (!plane.Raycast(ray, out distance)) {
            return;
        }
        centOffset = transform.position - ray.GetPoint(distance);
    }

    void OnMouseDrag() {
        // Shift line
        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (!plane.Raycast(ray, out distance)) {
            return;
        }
        var newPos = ray.GetPoint(distance) + centOffset;
        var offset = newPos - transform.position;

        // Determine whether to snap lines or not
        var ep1Pos = points.Item1.transform.position + offset;
        var ep2Pos = points.Item2.transform.position + offset;
        var snapEp1Pos = SegmentHelper.SnapToLines(ep1Pos, snapDist, id);
        var snapEp2Pos = SegmentHelper.SnapToLines(ep2Pos, snapDist, id);
        Vector3 snapOffset = Vector3.zero;
        if (snapEp1Pos != ep1Pos && snapEp2Pos != ep2Pos) {
            if (Vector3.Distance(snapEp1Pos, ep1Pos) <
                Vector3.Distance(snapEp2Pos, ep2Pos)) {
                snapOffset = snapEp1Pos - ep1Pos;
            } else {
                snapOffset = snapEp2Pos - ep2Pos; } } else if (snapEp1Pos != ep1Pos) {
            snapOffset = snapEp1Pos - ep1Pos;
        } else if (snapEp2Pos != ep2Pos) {
            snapOffset = snapEp2Pos - ep2Pos;
        }
        points.Item1.transform.position = ep1Pos + snapOffset;
        points.Item2.transform.position = ep2Pos + snapOffset;
        transform.position = newPos + snapOffset;
    }

    void OnMouseUp() {
        SegmentHelper.UpdateLineRepr(this);
    }
}
