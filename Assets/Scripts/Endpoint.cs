using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Endpoint : IDable {
    
    public List<Tuple<Endpoint, Segment>> connects = new List<Tuple<Endpoint, Segment>>();

    void Start() {
    }
	
	// Update is called once per frame
	void OnMouseDrag() {
        if (connects.Count == 0) {
            return;
        }
        UpdateLinesToPos(transform.position);
	}

    void OnMouseUp() {
        foreach (var pointAndSeg in connects) {
            SegmentHelper.UpdateLineRepr(pointAndSeg.Item2);
        }
    }

    // Specific Helpers
    public void UpdateLinesToPos(Vector3 pos) {
        foreach (var pointAndSeg in connects) {
            SegmentHelper.UpdateLine(pos,
                pointAndSeg.Item1.transform.position,
                pointAndSeg.Item2.transform);
        }
    }

    public HashSet<int> connectSegmentIdSet() {
        HashSet<int> segmentIds = new HashSet<int>();
        foreach (var pointAndSeg in connects) {
            segmentIds.Add(pointAndSeg.Item2.id);
        }
        return segmentIds;
    }
}
