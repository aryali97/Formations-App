using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdateFrame : MonoBehaviour
{
    [SerializeField]
    private GameObject scrollContent;

    void OnMouseUp() {
        int frameNum = scrollContent.transform.childCount - 1;
        FrameData.UpdateBallsInFrame(frameNum);
    }
}
