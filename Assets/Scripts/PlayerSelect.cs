using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    private bool alreadySelected;
    private float downTime;

    public static void UnselectAll() {
        foreach (IDable idable in GlobalVars.selected) {
            foreach (Transform child in idable.transform) {
                if (child.name.Contains("Player Light")) {
                    GameObject.Destroy(child.gameObject); 
                }
            }
        }
        GlobalVars.selected.Clear();
    }

    public static void Unselect(GameObject go) {
        foreach (Transform child in go.transform) {
            if (child.name.Contains("Player Light")) {
                GameObject.Destroy(child.gameObject); 
            }
        }
        GlobalVars.selected.Remove(go.GetComponent<IDable>());
    }

    public static void Select(GameObject go) {
        if (GlobalVars.selected.Contains(go.GetComponent<IDable>())) {
            return;
        }
        GlobalVars.selected.Add(go.GetComponent<IDable>());
        Light light = GameObject.Instantiate(
            Resources.Load<Light>("Prefabs/Player Light"));
        light.transform.SetParent(go.transform);
        light.transform.localPosition = new Vector3(0, 0, 0);
    }

    public static void SelectInRect(
        Vector3 mouseStart, 
        Vector3 mouseEnd,
        HashSet<IDable> preSelected) {
        //UnselectAll();
        Plane stagePlane = new Plane(Vector3.up, new Vector3(0, 0.5f, 0));
        Vector3 stageStart = DrawingScript.ScreenToPlane(stagePlane, mouseStart);
        Vector3 stageEnd = DrawingScript.ScreenToPlane(stagePlane, mouseEnd);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (GameObject player in players) {
            float minX = Math.Min(stageStart.x, stageEnd.x);
            float maxX = Math.Max(stageStart.x, stageEnd.x);
            float minZ = Math.Min(stageStart.z, stageEnd.z);
            float maxZ = Math.Max(stageStart.z, stageEnd.z);
            if (player.transform.position.x >= minX &&
                player.transform.position.x <= maxX &&
                player.transform.position.z >= minZ &&
                player.transform.position.z <= maxZ) {
                Select(player);
            } else if (!preSelected.Contains(player.GetComponent<IDable>())) {
                Unselect(player);
            }
        }
    }

    public static void MouseDownHandler(GameObject go) {
        if (Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift)) {
            if (!GlobalVars.selected.Contains(go.GetComponent<IDable>())) {
                Select(go);
            }
        } else {
            UnselectAll();
            Select(go);
        }
    }

    public static void MouseUpHandler(GameObject go) {
        if ((Input.GetKey(KeyCode.LeftShift) ||
             Input.GetKey(KeyCode.RightShift)) &&
            GlobalVars.selected.Contains(go.GetComponent<IDable>()) &&
            go.transform.position.y == 1.0f) {
            Unselect(go);
        }
    }

    void Start() {
        alreadySelected = false;
    }

    void OnMouseDown() {
        downTime = Time.time;
        alreadySelected = GlobalVars.selected.Contains(GetComponent<IDable>());
        if (Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift)) {
            if (!alreadySelected) {
                Select(gameObject);
            } 
        } else if (!alreadySelected) {
            UnselectAll();
            Select(gameObject);
        }
    }

    void OnMouseUp() {
        if (GlobalVars.selected.Contains(GetComponent<IDable>()) &&
            transform.position.y == 1.0f &&
            alreadySelected) {
            if (Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift)) {
                Unselect(gameObject);
            } else if ((Time.time - downTime) < 0.2f) {
                // Short press so select only this player
                UnselectAll();
                Select(gameObject);
            }
        }
    }
}
