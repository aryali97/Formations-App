using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDable : MonoBehaviour {
    
    public int id_;

    private float y;
    private Vector3 cent_offset;
    private Hoverable hover;

    void Awake() {
        id_ = GlobalVars.nextId;
        GlobalVars.nextId++;
    }

	// Use this for initialization
	void Start () {
        y = 0.6f;
        hover = GetComponent<Hoverable>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnMouseDown() {
        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (!plane.Raycast(ray, out distance)) {
            return;
        }
        cent_offset = transform.position - ray.GetPoint(distance);
    }

    void OnMouseDrag() {
        // Hacky fix since drag may make the cursor not hover
        hover.OnMouseEnter();

        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (!plane.Raycast(ray, out distance)) {
            return;
        }
        var newPos = ray.GetPoint(distance) + cent_offset;
        var offset = newPos - transform.position;
        transform.position = new Vector3(newPos.x,
                                         y,
                                         newPos.z);
        var endpoints = GlobalVars.GetEndpointsFromLine(this);
        var ep1Pos = endpoints.Item1.transform.position + offset;
        var ep2Pos = endpoints.Item2.transform.position + offset;
        endpoints.Item1.transform.position = ep1Pos;
        endpoints.Item2.transform.position = ep2Pos;
    }

    void OnMouseUp() {
        GlobalVars.UpdateLineRepr(this);
        // Hacky fix since drag may make the cursor not hover
        hover.OnMouseExit();
    }
}
