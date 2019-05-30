using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars {
    public static int nextId = 0;
    public static List<LineRepr> snapLines = new List<LineRepr>();
    /*
    public static Dictionary<Transform, Transform> pointToLine =
        new Dictionary<Transform, Transform>();
        */
    public static Dictionary<int, Endpoint> idToPoint =
        new Dictionary<int, Endpoint>();
    public static Dictionary<int, int> pointToLine =
        new Dictionary<int, int>();
    public static Dictionary<int, int> pointToPoint =
        new Dictionary<int, int>();

    public static Vector2 SnapToLines(Vector2 point, float snapDist) {
        float bestDist = snapDist;
        Vector2 closestPoint = point;
        List<int> snapLineIndex = new List<int>();
        for (int i = 0; i < snapLines.Count; ++i) {
            Vector2 newP = snapLines[i].ClosestPoint(point);
            float dist = Vector2.Distance(newP, point);
            if (dist < bestDist) {
                bestDist = dist;
                closestPoint = newP;
                snapLineIndex.Add(i);
            }
        }
        bestDist = snapDist;
        for (int i = 0; i < snapLines.Count; ++i) {
            for (int j = i + 1; j < snapLines.Count; ++j) {
                Vector2 newP = new Vector2(0, 0);
                if (LineRepr.Intersection(
                        snapLines[i],
                        snapLines[j],
                        ref newP) &&
                    Vector2.Distance(point, newP) < bestDist) {
                    bestDist = snapDist;
                    closestPoint = newP;
                }
            }
        }
        return closestPoint;
    }
}
