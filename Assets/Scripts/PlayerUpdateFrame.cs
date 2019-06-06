using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdateFrame : MonoBehaviour
{
    void OnMouseUp() {
        int frameNum = FrameData.scrollContent.transform.childCount - 1;
        FrameData.UpdateBallsInFrame(frameNum);
    }
}
