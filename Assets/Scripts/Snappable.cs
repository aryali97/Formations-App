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
        Debug.Log("IN START");
	}

    void OnMouseDown() {
        Debug.Log("In MouseDown");
    }

    /*
    static Vector2 SnapToLines(Vector2 point) {
        float bestDist = snapDist;
        Vector2 closestPoint = point;
        List<int> snapLineIndex = new List<int>();
        for (int i = 0; i < GlobalVars.snapLines.Count; ++i) {
            Vector2 newP = GlobalVars.snapLines[i].ClosestPoint(point);
            float dist = Vector2.Distance(newP, point);
            if (dist < bestDist) {
                bestDist = dist;
                closestPoint = newP;
                snapLineIndex.Add(i);
            }
        }
        bestDist = snapDist;
        for (int i = 0; i < GlobalVars.snapLines.Count; ++i) {
            for (int j = i + 1; j < GlobalVars.snapLines.Count; ++j) {
                Vector2 newP = new Vector2(0, 0);
                if (LineRepr.Intersection(
                        GlobalVars.snapLines[i],
                        GlobalVars.snapLines[j],
                        ref newP) &&
                    Vector2.Distance(point, newP) < bestDist) {
                    bestDist = snapDist;
                    closestPoint = newP;
                }
            }
        }
        return closestPoint;
    }
    */

    void OnMouseDrag() {
        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance)) {
            var newPos = ray.GetPoint(distance);
            if (snapToggle.isOn) {
                var snappedCoords = GlobalVars.SnapToLines(
                    new Vector2(newPos.x, newPos.z),
                    snapDist);
                transform.position = new Vector3(
                    snappedCoords.x,
                    y,
                    snappedCoords.y);
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
