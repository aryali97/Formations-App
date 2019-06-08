using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdateFrame : MonoBehaviour
{
    static int firstLayer = 8;
    static int nextLayer = firstLayer;

    void Start() {
        gameObject.layer = nextLayer;
        nextLayer++;
        for (int i = firstLayer; i < nextLayer; i++) {
            Physics.IgnoreLayerCollision(i, nextLayer);
        }
    }
    
    void OnMouseUp() {
        FrameData.UpdateBallsInFrame(FrameData.selectedFrame);
    }
}
