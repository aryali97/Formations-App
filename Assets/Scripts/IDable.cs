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
        if (plane.Raycast(ray, out distance)) {
            var newPos = ray.GetPoint(distance);
            var offset = newPos - transform.position + cent_offset;
            transform.position = new Vector3(newPos.x + cent_offset.x, y, newPos.z + cent_offset.z);
            var endpoints = GlobalVars.GetEndpointsFromLine(this);
            endpoints.Item1.transform.position = 
                endpoints.Item1.transform.position + offset;
            endpoints.Item2.transform.position = 
                endpoints.Item2.transform.position + offset;
        }
    }

    void OnMouseUp() {
        // Hacky fix since drag may make the cursor not hover
        hover.OnMouseExit();
    }
}
