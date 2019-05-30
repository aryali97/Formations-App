using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars {
    public static int nextId = 0;

    public static List<LineRepr> snapLines = new List<LineRepr>();
    public static List<Endpoint> pointsList = new List<Endpoint>();
    public static List<IDable> linesList = new List<IDable>();
    public static Dictionary<int, int> pointToLine =
        new Dictionary<int, int>();
    public static Dictionary<int, int> pointToPoint =
        new Dictionary<int, int>();


    /*
    Update line representation from line
    */
    public static void UpdateLineRepr(IDable line) {
        if (line == null) {
            return;
        }
        for (int i = 8; i < snapLines.Count; i++) {
            if (snapLines[i].line_id_ != line.id_) {
                continue;
            }
            float half_length = line.transform.localScale.z * 5.0f;
            if (line.transform.rotation.eulerAngles.y != 90 && 
                line.transform.rotation.eulerAngles.y != 270) {
                float p1x = (float)(Math.Sin(line.transform.rotation.eulerAngles.y * Mathf.Deg2Rad))
                    * half_length + line.transform.position.x;
                float p1z = (float)(Math.Cos(line.transform.rotation.eulerAngles.y * Mathf.Deg2Rad))
                    * half_length + line.transform.position.z;
                float p2x = line.transform.position.x - (p1x - line.transform.position.x);
                float p2z = line.transform.position.z - (p1z - line.transform.position.z);
                Debug.Log("Guess points are (" + p1x + ", " + p1z + ") and (" +
                          p2x + ", " + p2z + ")");
                float m = (p2z - p1z)/(p2x - p1x);
                float b = p2z - m * p2x;
                Debug.Log("M and B: " + m + ", " + b);
                snapLines[i] = new LineRepr(
                    m,
                    b,
                    new Vector2(p1x, p1z),
                    new Vector2(p2x, p2z));
            } else {
                float p1x = line.transform.position.x;
                float p1z = line.transform.position.z - half_length;
                float p2z = line.transform.position.z + half_length;
                snapLines[i] = new LineRepr(p1x, p1z, p2z);
            }
            snapLines[i].line_id_ = line.id_;
        }
    }

    /*
    Update line position from endpoints
    */
    public static void UpdateLine(Vector3 start,
                                  Vector3 end,
                                  Transform line) {
        float dist = (float)Math.Sqrt(
            Math.Pow(start.x - end.x, 2) +
            Math.Pow(start.z - end.z, 2));
        float angle = Mathf.Atan2(
            end.x - start.x,
            end.z - start.z) * Mathf.Rad2Deg;
        line.position = (end + start)/2.0f;
        line.rotation = Quaternion.Euler(0, angle, 0);
        line.localScale = new Vector3(
            line.localScale.x,
            line.localScale.y,
            dist/10.0f);
    }

    /*
    Finds IDable line connected directly to point with given id
    */
    public static IDable FindConnectedLine(int id) {
        if (!pointToLine.ContainsKey(id)) {
            return null;
        }
        return FindLine(pointToLine[id]);
    }

    /*
    Finds IDable line with given id
    */
    public static IDable FindLine(int id) {
        foreach (IDable line in linesList) {
            if (line.id_ == id) {
                return line;
            }
        }
        return null;
    }

    /*
    Finds Endpoint connected one away, else returns null
    */
    public static Endpoint FindConnectedPoint(int id) {
        if (!pointToPoint.ContainsKey(id)) {
            return null;
        }
        return FindPoint(pointToPoint[id]);
    }

    /*
    Finds Endpoint with given id
    */
    public static Endpoint FindPoint(int id) {
        foreach (Endpoint ep in pointsList) {
            if (ep.id_ == id) {
                return ep;
            }
        }
        return null;
    }

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
