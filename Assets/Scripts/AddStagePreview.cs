using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddStagePreview : MonoBehaviour
{
    [SerializeField]
    private Button previewTemplate;

    void Awake() {
        FrameData.UpdateBallsInFrame(0);
    }

    public void OnButtonClick() {
        var frameToDuplicate = Resources.Load<SelectFrame>("Prefabs/Stage Preview");
        var newButton = Instantiate(frameToDuplicate);
        newButton.transform.SetParent(FrameData.scrollContent.transform, false);
        if (FrameData.selectedFrame >= 0) {
            newButton.transform.SetSiblingIndex(FrameData.selectedFrame + 1);
        }
        newButton.transform.GetChild(1).GetComponent<Text>().text = "#" +
            (newButton.transform.GetSiblingIndex() + 1);
        newButton.transform.GetChild(2).GetComponent<InputField>().text =
            FrameData.scrollContent.transform.GetChild(FrameData.selectedFrame)
                .GetChild(2).GetComponent<InputField>().text;
        
        int lastFrame = (FrameData.selectedFrame >= 0 ?
            FrameData.selectedFrame :
            FrameData.scrollContent.transform.childCount - 2);
        FrameData.PushBackFrameData(lastFrame);
        FrameData.CreateEmptyMovements(lastFrame);
        FrameData.UpdateBallsInFrame(newButton.transform.GetSiblingIndex());

        newButton.Select();
    }
}
