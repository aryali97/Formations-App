using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerEdit : MonoBehaviour
{
    static void DeleteOldLines() {
        SegmentHelper.snapLines.RemoveRange(0,
            (GlobalVars.horizSecs - 1) + (GlobalVars.vertSecs - 1));

        foreach (var line in GameObject.FindGameObjectsWithTag("Marker Line")) {
            Destroy(line);
        }
    }

    public void OnHorizontalEndEdit() {
        string text = GetComponent<InputField>().text;
        if (String.IsNullOrEmpty(text)) {
            GetComponent<InputField>().text = "" + GlobalVars.horizSecs;
            return;
        }
        DeleteOldLines();
        GlobalVars.horizSecs = Int32.Parse(text);
        DrawingScript.SetUpMarkerLines();
    }

    public void OnVerticalEndEdit() {
        string text = GetComponent<InputField>().text;
        if (String.IsNullOrEmpty(text)) {
            GetComponent<InputField>().text = "" + GlobalVars.vertSecs;
            return;
        }
        DeleteOldLines();
        GlobalVars.vertSecs = Int32.Parse(text);
        DrawingScript.SetUpMarkerLines();
    }
}
