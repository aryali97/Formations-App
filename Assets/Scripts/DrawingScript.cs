using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class DrawingScript : MonoBehaviour {

    public float drawingPlaneY;
    public Transform drawnLine;
    public Toggle drawn_lines_toggle_;

    private Endpoint epTemplate;
    private IDable lineTemplate;
    
    private Vector3 start;
    private Transform startPoint;
    private Transform endPoint;
    private IDable currentLine;
    private Plane drawingPlane;
    private bool shouldDraw;

	// Use this for initialization
	void Start () {
        startPoint = null;
        endPoint = null;
        currentLine = null;
	    drawingPlane = new Plane(
            Vector3.up,
            new Vector3(0, drawingPlaneY, 0));
        for (int i = -2; i <= 2; ++i) {
            GlobalVars.snapLines.Add(new LineRepr(i * 2.0f));
        }
        for (int i = -1; i <= 1; ++i) {
            GlobalVars.snapLines.Add(new LineRepr(0, 2.0f * i));
        }
        epTemplate = Resources.Load<Endpoint>("Prefabs/Endpoint Circle");
        lineTemplate = Resources.Load<IDable>("Prefabs/Drawn Line");
	}

    Vector3 ScreenToPlane(Plane plane) { Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance)) {
            return ray.GetPoint(distance);        
        }
        Debug.LogError("Couldn't raycast screen click to point on plane");
        return Vector3.zero;
    }

    bool HitEndPoint() {
        return false;
    }

    void OnMouseDown() {
        if (!drawn_lines_toggle_.isOn) {
            shouldDraw = false;
            return;
        }

        Plane epplane = new Plane(
            Vector3.up,
            new Vector3(0, drawingPlaneY, 0));
        Vector3 epplanepoint = ScreenToPlane(epplane);
        RaycastHit2D hit = Physics2D.Raycast(epplanepoint, Vector2.zero);
        if (hit && hit.collider.name.Contains("Endpoint")) {
            shouldDraw = Vector3.Distance(hit.transform.position, epplanepoint) > 1.0f;
        } else {
            shouldDraw = true;
        }
        if (shouldDraw) {
            start = ScreenToPlane(drawingPlane);
        }
    }

    void AddLineToDicts(Endpoint startEp, Endpoint endEp, IDable line) {
        GlobalVars.pointToLine.Add(startEp.id_, line.id_);
        GlobalVars.pointToLine.Add(endEp.id_, line.id_);
        GlobalVars.pointToPoint.Add(startEp.id_, endEp.id_);
        GlobalVars.pointToPoint.Add(endEp.id_, startEp.id_);
        GlobalVars.pointsList.Add(startEp);
        GlobalVars.pointsList.Add(endEp);
        GlobalVars.linesList.Add(line);
    }

    void OnMouseDrag() {
        if (!shouldDraw) {
            return;
        }
        Vector3 end = ScreenToPlane(drawingPlane);
        if (end == start) {
            return;
        }
        Vector2 end2d = GlobalVars.SnapToLines(new Vector2(end.x, end.z), 0.3f);
        end = new Vector3(end2d.x, end.y, end2d.y);
        Vector3 mid = (end + start)/2.0f;
        float dist = (float)Math.Sqrt(
            Math.Pow(start.x - end.x, 2) +
            Math.Pow(start.z - end.z, 2));
        float angle = Mathf.Atan2(
            end.x - start.x,
            end.z - start.z) * Mathf.Rad2Deg;
        if (startPoint == null) {
            Endpoint startEp =
                Instantiate(epTemplate, start, Quaternion.Euler(0, 0, 0));
            startPoint = startEp.transform;
            Endpoint endEp =
                Instantiate(epTemplate, start, Quaternion.Euler(0, 0, 0));
            endPoint = endEp.transform;
            currentLine = 
                Instantiate(lineTemplate, mid, Quaternion.Euler(0, angle, 0));
            AddLineToDicts(startEp, endEp, currentLine);
        }
        endPoint.position = end;
        currentLine.transform.position = mid;
        currentLine.transform.rotation = Quaternion.Euler(0, angle, 0);
        currentLine.transform.localScale = new Vector3(
            currentLine.transform.localScale.x,
            currentLine.transform.localScale.y,
            dist/10.0f);
    }

    void OnMouseUp() {
        if (!shouldDraw) {
            return;
        }
        Vector3 end = ScreenToPlane(drawingPlane);
        if (end != start) {
            if (end.x == start.x) {
                GlobalVars.snapLines.Add(
                    new LineRepr(start.x, start.y, end.y));
                GlobalVars.snapLines.Last().line_id_ = currentLine.id_;
            } else {
                float m = (end.z - start.z)/(end.x - start.x);
                // Y = mX + b
                // b = Y - mX
                float b = end.z - m * end.x;
                GlobalVars.snapLines.Add(
                    new LineRepr(
                        m,
                        b,
                        new Vector2(start.x, start.z),
                        new Vector2(end.x, end.z)));
                GlobalVars.snapLines.Last().line_id_ = currentLine.id_;
            }
            startPoint = null;
            endPoint = null;
            //currentLine = null;
        }
        currentLine = null;
    }

    static Vector3 V2ToV3(Vector2 v2) {
        return new Vector3(v2.x, 0.0f, v2.y);
    }

    void CreateCircle(Vector3 center) {
		GameObject circle = new GameObject("Circle");

        const float radius = 0.2f;
		const float segmentOffset = 40f;
        const float segmentMultiplier = 5f;
        var numSegments = (int) (radius * segmentMultiplier + segmentOffset);

        // Create an array of points arround a cricle
        var circleVertices = Enumerable.Range(0, numSegments)
            .Select(i => {
                var theta = 2 * Mathf.PI * i / numSegments;
                return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * radius;
            })
            .ToArray();
		
        // Find all the triangles in the shape
        var triangles = new Triangulator(circleVertices).Triangulate();
		
        // Assign each vertex the fill color
        var colors = Enumerable.Repeat(Color.blue, circleVertices.Length).ToArray();

        var mesh = new Mesh {
            name = "Circle",
            vertices = System.Array.ConvertAll<Vector2, Vector3> (
                circleVertices,
                V2ToV3),
            triangles = triangles,
            colors = colors
        };
		
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
		
		var filter = circle.AddComponent<MeshFilter>();
        filter.mesh = mesh;		
        // AssetDatabase.CreateAsset(mesh, "Assets/Meshes/CircleMeshFilter.obj"); 
        // AssetDatabase.SaveAssets();

		var meshRenderer = circle.AddComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load("Materials/Red_Draw.mat", typeof(Material)) as Material;

		Instantiate(circle, center, Quaternion.Euler(0, 0, 0));
        // UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Prefabs/Endpoint Circle.prefab");
        // PrefabUtility.ReplacePrefab(circle, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
