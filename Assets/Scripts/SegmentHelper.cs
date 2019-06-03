using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SegmentHelper {
    public static List<LineRepr> snapLines = new List<LineRepr>();
    public static List<Endpoint> pointsList = new List<Endpoint>();
    public static List<Segment> linesList = new List<Segment>();
    public static Dictionary<int, int> pointToPoint =
        new Dictionary<int, int>();
    public static Dictionary<int, Tuple<int, int>> lineToPoints =
        new Dictionary<int, Tuple<int, int>>();


    /*
    Add Segment to dictionaries for other use
    */
    public static void AddSegmentToDicts(Endpoint startEp,
                                         Endpoint endEp,
                                         Segment line) {
        pointsList.Add(startEp);
        pointsList.Add(endEp);
        linesList.Add(line);
        pointToPoint.Add(startEp.id, endEp.id);
        pointToPoint.Add(endEp.id, startEp.id);
        lineToPoints.Add(line.id,
            new Tuple<int, int>(startEp.id, endEp.id));
    }

    /*
    Discards y axis
    */
    public static Vector2 V3toV2(Vector3 vec) {
        return new Vector2(vec.x, vec.z);
    }

    /*
    Get Endpoints from IDable line
    */
    public static Tuple<Endpoint, Endpoint> GetEndpointsFromLine(IDable line) {
        if (line == null) {
            return null;
        }
        Endpoint ep1 = null;
        Endpoint ep2 = null;
        Tuple<int, int> pointIds = lineToPoints[line.id];
        foreach (Endpoint ep in pointsList) {
            if (ep.id == pointIds.Item1) {
                ep1 = ep;
            } else if (ep.id == pointIds.Item2) {
                ep2 = ep;
            }
        }
        return new Tuple<Endpoint, Endpoint>(ep1, ep2);
    }

    /*
    Update line representation from line
    */
    public static void UpdateLineRepr(Segment line) {
        if (line == null) {
            return;
        }
        int markerCount = GlobalVars.horizSecs + GlobalVars.vertSecs - 2;
        for (int i = markerCount; i < snapLines.Count; i++) {
            if (snapLines[i].lineId != line.id) {
                continue;
            }
            snapLines[i] = CreateFromSegment(line);
        }
    }

    /*
    Create LineRepr from Segment
    */
    public static LineRepr CreateFromSegment(Segment line) {
        LineRepr repr;
        float halfLength = line.transform.localScale.z * 5.0f;
        if (line.transform.rotation.eulerAngles.y != 90 && 
            line.transform.rotation.eulerAngles.y != 270) {
            float p1x = (float)(Math.Sin(line.transform.rotation.eulerAngles.y * Mathf.Deg2Rad))
                * halfLength + line.transform.position.x;
            float p1z = (float)(Math.Cos(line.transform.rotation.eulerAngles.y * Mathf.Deg2Rad))
                * halfLength + line.transform.position.z;
            float p2x = line.transform.position.x - (p1x - line.transform.position.x);
            float p2z = line.transform.position.z - (p1z - line.transform.position.z);
            float m = (p2z - p1z)/(p2x - p1x);
            float b = p2z - m * p2x;
            repr = new LineRepr(
                m,
                b,
                new Vector2(p1x, p1z),
                new Vector2(p2x, p2z));
        } else {
            float p1x = line.transform.position.x;
            float p1z = line.transform.position.z - halfLength;
            float p2z = line.transform.position.z + halfLength;
            repr = new LineRepr(p1x, p1z, p2z);
        }
        repr.lineId = line.id;
        return repr;
    }

    /*
    Create drawn line and endpoints from vector3 endpoints
    public static void CreateSegment(Vector3 start, Vector3 end,
        Endpoint startEp, Endpoint endEp, Segment line) {
        Vector3 mid = (end + start)/2.0f;
        float angle = Mathf.Atan2(
            end.x - start.x,
            end.z - start.z) * Mathf.Rad2Deg;
        Endpoint epTemplate = Resources.Load<Endpoint>("Prefabs/Endpoint Circle");
        startEp =
            GameObject.Instantiate(epTemplate, start, Quaternion.Euler(0, 0, 0));
        endEp =
            GameObject.Instantiate(epTemplate, start, Quaternion.Euler(0, 0, 0));
        line = 
            GameObject.Instantiate(Resources.Load<Segment>("Prefabs/Drawn Line"),
                mid,
                Quaternion.Euler(0, angle, 0));
        AddSegmentToDicts(startEp, endEp, line);
    }
    */

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
    Finds IDable line with given id
    */
    public static Segment FindLine(int id) {
        foreach (Segment line in linesList) {
            if (line.id == id) {
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
            if (ep.id == id) {
                return ep;
            }
        }
        return null;
    }

    public static Vector3 SnapToLines(Vector3 point,
                                      float snapDist,
                                      HashSet<int> omitIds = null) {
        var snap2d = SnapToLines(V3toV2(point), snapDist, omitIds);
        return new Vector3(snap2d.x, point.y, snap2d.y);
    }

    public static Vector2 SnapToLines(Vector2 point,
                                      float snapDist,
                                      HashSet<int> omitIds = null) {
        float bestDist = snapDist;
        Vector2 closestPoint = point;
        List<int> snapLineIndex = new List<int>();
        //TODO: Change if grid line number changes
        Toggle linesToggle = GameObject.FindWithTag("Shown Drawn Lines Toggle").
            GetComponent<Toggle>();
        int markerCount = GlobalVars.horizSecs + GlobalVars.vertSecs - 2;
        int endCount = linesToggle.isOn ? snapLines.Count : markerCount;
        for (int i = 0; i < endCount; ++i) {
            if (omitIds != null && omitIds.Contains(snapLines[i].lineId)) {
                continue;
            }
            Vector2 newP = snapLines[i].ClosestPoint(point);
            float dist = Vector2.Distance(newP, point);
            if (dist < bestDist) {
                bestDist = dist;
                closestPoint = newP;
                snapLineIndex.Add(i);
            }
        }
        bestDist = snapDist;
        for (int i = 0; i < endCount; ++i) {
            if (omitIds != null && omitIds.Contains(snapLines[i].lineId)) {
                continue;
            }
            for (int j = i + 1; j < endCount; ++j) {
                if (omitIds != null && omitIds.Contains(snapLines[i].lineId)) {
                    continue;
                }
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

    public static Vector3 SnapToLines(Vector3 point, float snapDist, int omitId) {
        var snap2d = SnapToLines(V3toV2(point), snapDist, omitId);
        return new Vector3(snap2d.x, point.y, snap2d.y);
    }

    public static Vector2 SnapToLines(Vector2 point, float snapDist, int omitId) {
        float bestDist = snapDist;
        Vector2 closestPoint = point;
        List<int> snapLineIndex = new List<int>();
        //TODO: Change if grid line number changes
        Toggle linesToggle = GameObject.FindWithTag("Shown Drawn Lines Toggle").
            GetComponent<Toggle>();
        int markerCount = GlobalVars.horizSecs + GlobalVars.vertSecs - 2;
        int endCount = linesToggle.isOn ? snapLines.Count : markerCount;
        for (int i = 0; i < endCount; ++i) {
            if (snapLines[i].lineId == omitId) {
                continue;
            }
            Vector2 newP = snapLines[i].ClosestPoint(point);
            float dist = Vector2.Distance(newP, point);
            if (dist < bestDist) {
                bestDist = dist;
                closestPoint = newP;
                snapLineIndex.Add(i);
            }
        }
        bestDist = snapDist;
        for (int i = 0; i < endCount; ++i) {
            if (snapLines[i].lineId == omitId) {
                continue;
            }
            for (int j = i + 1; j < endCount; ++j) {
                if (snapLines[j].lineId == omitId) {
                    continue;
                }
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
