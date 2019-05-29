using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars {
    public static int nextPointId = 0;
    public static int nextLineId = 0;
    public static List<LineRepr> snapLines = new List<LineRepr>();
    /*
    public static Dictionary<Transform, Transform> pointToLine =
        new Dictionary<Transform, Transform>();
        */
    public static Dictionary<int, int> pointToLine =
        new Dictionary<int, int>();
    public static Dictionary<int, int> pointToPoint =
        new Dictionary<int, int>();
}
