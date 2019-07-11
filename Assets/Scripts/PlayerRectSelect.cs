using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRectSelect : MonoBehaviour
{
    private Vector3 startPos;
    private Image rect;
    private HashSet<IDable> preSelected;

    // Start is called before the first frame update
    void Start()
    {}

    void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (EventSystem.current.currentSelectedGameObject != null) return;
        startPos = Input.mousePosition;
        if (!Input.GetKey(KeyCode.LeftShift) &&
            !Input.GetKey(KeyCode.RightShift)) {
            preSelected = new HashSet<IDable>();
            PlayerSelect.UnselectAll();
        } else {
            preSelected = new HashSet<IDable>(GlobalVars.selected); 
        }
    }

    void OnMouseDrag() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (EventSystem.current.currentSelectedGameObject != null) return;
        if (Input.mousePosition == startPos) {
            // Delete rect
            Destroy(rect);
            return;
        }
        if (rect == null) {
            rect = Instantiate(Resources.Load<Image>("Prefabs/Select Rectangle"));
            rect.transform.SetParent(GameObject.Find("Canvas").transform);
        }
        Vector3 endPos = Input.mousePosition;
        rect.transform.position = (endPos + startPos) / 2.0f;
        rect.rectTransform.sizeDelta = new Vector2(
            Math.Abs((endPos - startPos).x),
            Math.Abs((endPos - startPos).y));
        PlayerSelect.SelectInRect(startPos, endPos, preSelected);
    }

    void OnMouseUp() {
        if (rect != null) {
            Destroy(rect.gameObject);
            rect = null;
        }
    }
}
