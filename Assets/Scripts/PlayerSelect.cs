using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSelect
{
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
        GlobalVars.selected.Add(go.GetComponent<IDable>());
        Light light = GameObject.Instantiate(
            Resources.Load<Light>("Prefabs/Player Light"));
        light.transform.SetParent(go.transform);
        light.transform.localPosition = new Vector3(0, 0, 0);
    }

    public static void ClickHandler(GameObject go) {
        if (Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift)) {
            if (GlobalVars.selected.Contains(go.GetComponent<IDable>())) {
                Unselect(go);
            } else {
                Select(go);
            }
        } else {
            UnselectAll();
            Select(go);
        }
    }
}
