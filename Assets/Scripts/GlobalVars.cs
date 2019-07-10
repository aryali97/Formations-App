using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GlobalVars {
    public static float horizSize;
    public static float vertSize;
    public static int horizSecs = 3;
    public static int vertSecs = 3;
    public static float markerLineY = 0.501f;

    // Colors
    public static Color defaultColor = Color.red;
    public static Color hoverColor = new Vector4(1.0f, 0.0f, 0.5f);
    public static Color activeColor = Color.cyan;

    // Selected balls
    public static HashSet<IDable> selected = new HashSet<IDable>();
}
