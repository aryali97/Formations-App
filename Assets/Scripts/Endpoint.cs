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
        foreach (var pointAndSeg in connects) {
            SegmentHelper.UpdateLine(this.transform.position,
                pointAndSeg.Item1.transform.position,
                pointAndSeg.Item2.transform);
        }
	}

    void OnMouseUp() {
        foreach (var pointAndSeg in connects) {
            SegmentHelper.UpdateLineRepr(pointAndSeg.Item2);
        }
    }

    // Specific Helpers
    public HashSet<int> connectSegmentIdSet() {
        HashSet<int> segmentIds = new HashSet<int>();
        foreach (var pointAndSeg in connects) {
            segmentIds.Add(pointAndSeg.Item2.id);
        }
        return segmentIds;
    }
}
