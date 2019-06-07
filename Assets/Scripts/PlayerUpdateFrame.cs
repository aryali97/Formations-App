using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdateFrame : MonoBehaviour
{
    void OnMouseUp() {
        FrameData.UpdateBallsInFrame(FrameData.selectedFrame);
    }
}
