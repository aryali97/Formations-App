using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFrame : MonoBehaviour
{
    public void Select()
    {
        if (FrameData.selectedFrame >= 0) {
            FrameData.GetFrameTransform(FrameData.selectedFrame)
                .GetComponent<SelectFrame>().Unselect();
        }

        var selectBorder = Instantiate(Resources.Load<RawImage>(
            "Prefabs/Select Frame Border"));
        selectBorder.transform.SetParent(transform, false);
        selectBorder.GetComponent<RectTransform>().localPosition = Vector3.zero;
        FrameData.selectedFrame = transform.GetSiblingIndex();
        FrameData.SetStageByFrame(FrameData.selectedFrame);
    }

    // Update is called once per frame
    public void Unselect()
    {
        if (FrameData.selectedFrame < 0) {
            return;
        }

        Transform oldSelected = FrameData.GetFrameTransform(
            FrameData.selectedFrame);
        if (oldSelected != null) {
            foreach (Transform child in oldSelected) {
                if (child.name.Contains("Select Frame Border")) {
                    Destroy(child.gameObject);
                }
            }
        }
        FrameData.selectedFrame = -1;
    }
}
