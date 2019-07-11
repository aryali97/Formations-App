using System;
using System.Collections; using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class DrawingScript : MonoBehaviour {

    public float drawingPlaneY;
    public Transform drawnLine;
    public Toggle drawnLinesToggle;

    private Endpoint epTemplate;
    private Segment lineTemplate;
    
    public Drawable selected;
    private Vector3 start;
    private Endpoint startPoint;
    private Transform endPoint;
    private Segment currentLine;
    private Plane drawingPlane;
    private bool drawingLine;
    private bool shouldDraw;
    private bool rightClickDownInPrev;
    private Vector3 rightMouseDownPos;

    // Used by height and width edits
    public static void SetUpMarkerLines() {
        Segment stageMarkerTemplate = Resources.Load<Segment>("Prefabs/Marker Line");
        float xDist = GlobalVars.horizSize / GlobalVars.horizSecs; 
        for (int i = 1; i < GlobalVars.horizSecs; i++) {
            SegmentHelper.snapLines.Insert(0, new LineRepr(
                -1 * GlobalVars.horizSize / 2.0f + xDist * i));
            var segment = 
                Instantiate(stageMarkerTemplate,
                            new Vector3(-1 * GlobalVars.horizSize / 2.0f + xDist * i,
                                        GlobalVars.markerLineY,
                                        0),
                            Quaternion.Euler(0, 0, 0));
            segment.id = 0;
        }
        float yDist = GlobalVars.vertSize / GlobalVars.vertSecs;
        for (int i = 1; i < GlobalVars.vertSecs; i++) {
            SegmentHelper.snapLines.Insert(0, new LineRepr(
                0,
                -1 * GlobalVars.vertSize / 2.0f + yDist * i));
            var horizLineTrans = Instantiate(
                stageMarkerTemplate,
                new Vector3(0,
                            GlobalVars.markerLineY,
                            -1 * GlobalVars.vertSize / 2.0f + yDist * i),
                Quaternion.Euler(0, 90, 0));
            horizLineTrans.transform.localScale = new Vector3(
                horizLineTrans.transform.localScale.x,
                horizLineTrans.transform.localScale.y,
                GlobalVars.horizSize / 10.0f);
            horizLineTrans.id = 0;
        }
    }

	// Use this for initialization
	void Start () {
        drawingLine = false;
        endPoint = null;
        currentLine = null;
        rightClickDownInPrev = false;
        selected = null;
	    drawingPlane = new Plane(
            Vector3.up,
            new Vector3(0, drawingPlaneY, 0));

        GlobalVars.horizSize = GameObject.FindWithTag("Stage").transform.localScale.x;
        GlobalVars.vertSize = GameObject.FindWithTag("Stage").transform.localScale.z;
        if (Application.isPlaying) {
            SetUpMarkerLines();
        }
        epTemplate = Resources.Load<Endpoint>("Prefabs/Endpoint Circle");
        lineTemplate = Resources.Load<Segment>("Prefabs/Drawn Line");
	}

    public static Vector3 ScreenToPlane(Plane plane,
        Vector3 mousePos = default(Vector3)) {
        if (mousePos == Vector3.zero) {
            mousePos = Input.mousePosition;
        }
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
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
        //PlayerSelect.UnselectAll();
    }

    void OnMouseDrag() {
    }

    void OnMouseUp() {
    }

	// Update is called once per frame
	void Update () {
        MouseRightHandler();
        UnselectHandler();
        DeleteHandler();
	}

    void MouseRightHandler() {
        var eventSystem = GameObject.Find("EventSystem")
            .GetComponent<EventSystem>();
        if (eventSystem.IsPointerOverGameObject()) return;
        if (eventSystem.currentSelectedGameObject != null) return;
        if (Input.GetMouseButton(1)) {
            if (!rightClickDownInPrev) {
                rightMouseDownPos = Input.mousePosition;
                OnMouseRightDown(); 
            } else if (Input.mousePosition != rightMouseDownPos)  {
                OnMouseRightDrag(); 
            }
            rightClickDownInPrev = true;
        } else {
            if (rightClickDownInPrev) {
                rightMouseDownPos = Vector3.zero; 
                OnMouseRightUp();
            }
            rightClickDownInPrev = false;
        }
    }

    void OnMouseRightDown() {
        if (!drawnLinesToggle.isOn) {
            shouldDraw = false;
            return;
        }
        shouldDraw = true;
        Plane epplane = new Plane(
            Vector3.up,
            new Vector3(0, drawingPlaneY, 0));
        Vector3 epplanepoint = ScreenToPlane(epplane);
        RaycastHit2D hit = Physics2D.Raycast(epplanepoint, Vector2.zero);
        if (shouldDraw) {
            if (hit && hit.collider.name.Contains("Endpoint") &&
                Vector3.Distance(hit.transform.position, epplanepoint) <= 1.0f) {
                startPoint = hit.collider.gameObject.GetComponent<Endpoint>();
                start = startPoint.transform.position;
            } else { 
                
                start = ScreenToPlane(drawingPlane);
            }
        }
    }

    void OnMouseRightDrag() {
        if (!shouldDraw) {
            return;
        }
        Vector3 end = ScreenToPlane(drawingPlane);
        if (end == start) {
            return;
        }
        
        Vector2 preSnapEnd2d = new Vector2(end.x, end.z);
        if (Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift)) {
            LineRepr horizLine = new LineRepr(0, start.z);
            LineRepr vertLine = new LineRepr(start.x);
            Vector2 horizClosest = horizLine.ClosestPoint(preSnapEnd2d);
            Vector2 vertClosest = vertLine.ClosestPoint(preSnapEnd2d);
            if (Vector2.Distance(horizClosest, preSnapEnd2d) <
                Vector2.Distance(vertClosest, preSnapEnd2d)) {
                preSnapEnd2d = horizClosest;
            } else {
                preSnapEnd2d = vertClosest;
            }
        }

        Vector2 end2d = SegmentHelper.SnapToLines(preSnapEnd2d, 0.3f);
        end = new Vector3(end2d.x, end.y, end2d.y);
        if (!drawingLine) {
            Vector3 mid = (end + start)/2.0f;
            float angle = Mathf.Atan2(
                end.x - start.x,
                end.z - start.z) * Mathf.Rad2Deg;
            Endpoint startEp =
                startPoint != null ?
                startPoint :
                Instantiate(epTemplate, start, Quaternion.Euler(0, 0, 0));
            Endpoint endEp =
                Instantiate(epTemplate, start, Quaternion.Euler(0, 0, 0));
            endPoint = endEp.transform;
            currentLine = 
                Instantiate(lineTemplate, mid, Quaternion.Euler(0, angle, 0));
            SegmentHelper.AddSegmentToDicts(startEp, endEp, currentLine);
            startEp.connects.Add(new Tuple<Endpoint, Segment>(endEp, currentLine)); 
            endEp.connects.Add(new Tuple<Endpoint, Segment>(startEp, currentLine));
            currentLine.points = new Tuple<Endpoint, Endpoint>(startEp, endEp);
            drawingLine = true;            
        }
        endPoint.position = end;
        SegmentHelper.UpdateLine(start, end, currentLine.transform);
    }

    void OnMouseRightUp() {
        if (!shouldDraw) {
            return;
        }
        Vector3 end = ScreenToPlane(drawingPlane);
        if (end != start) {
            SegmentHelper.snapLines.Add(SegmentHelper.CreateFromSegment(currentLine));
        }
        drawingLine = false;
        endPoint = null;
        currentLine = null;
        startPoint = null;
    }

    void UnselectHandler() {
        if (!Input.GetKey(KeyCode.Escape)) {
            return;
        }
        PlayerSelect.UnselectAll();
        if (selected != null) {
            selected.Unselect();
            selected = null;
        }
    }

    void DeleteHandler() {
        if (selected == null || !Input.GetKey(KeyCode.Backspace)) {
            return;
        }
        Destroy(selected);
    }
}
