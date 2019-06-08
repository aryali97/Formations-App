using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveStagePreview : MonoBehaviour
{
    public void RemoveFrame()
    {
        if (FrameData.selectedFrame < 0 ||
            FrameData.scrollContent.transform.childCount <= 1) {
            Debug.Log("ERROR: RemoveFrame() (Can't remove without selected " +
                      "frame or more than one frame)");
            return;
        }

        // Update Movements
        if (FrameData.selectedFrame != 0 &&
            FrameData.selectedFrame != FrameData.scrollContent.transform
                .childCount - 1) {
            var balls = GameObject.FindGameObjectsWithTag("Player Ball");
            foreach (var ball in balls) {
                int ballId = ball.GetComponent<IDable>().id;
                FrameData.frameMovements[FrameData.selectedFrame - 1]
                    [ballId].end =
                    FrameData.framePlayerPositions[FrameData.selectedFrame + 1]
                        [ballId];
                FrameData.frameMovements[FrameData.selectedFrame - 1]
                    [ballId].beats =
                    FrameData.GetBeatFromFrame(FrameData.selectedFrame + 1);
            }
        }
        for (int i = FrameData.selectedFrame;
             i < (FrameData.scrollContent.transform.childCount - 2);
             i++) {
            FrameData.frameMovements[i] = new Dictionary<int, Movement>(
                FrameData.frameMovements[i + 1]);
        }
        FrameData.frameMovements.Remove(
            FrameData.scrollContent.transform.childCount - 2);

        // Update frame numbers
        for (int i = FrameData.selectedFrame + 1;
             i < FrameData.scrollContent.transform.childCount;
             i++) {
             FrameData.GetFrameTransform(i).GetChild(1).GetComponent<Text>()
                .text = "#" + i;
        }

        // Update frame positions
        for (int i = FrameData.selectedFrame;
             i < (FrameData.scrollContent.transform.childCount - 1);
             i++) {
            FrameData.framePlayerPositions[i] = new Dictionary<int, Vector3>(
                FrameData.framePlayerPositions[i + 1]);
        }
        FrameData.framePlayerPositions.Remove(
            FrameData.scrollContent.transform.childCount - 1);

        // Update scrollContent and selectedFrame
        bool lastFrameSelected = (FrameData.selectedFrame ==
            FrameData.scrollContent.transform.childCount - 1);
        Destroy(FrameData.GetFrameTransform(FrameData.selectedFrame).gameObject);
        if (lastFrameSelected) {
            FrameData.selectedFrame--;
        }
        FrameData.GetFrameTransform(FrameData.selectedFrame)
            .GetComponent<SelectFrame>().Select();
    }
}
