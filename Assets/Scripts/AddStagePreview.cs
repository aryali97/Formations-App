﻿using System;
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
        // Change to selected from to duplicate
        var frameToDuplicate = 
            FrameData.scrollContent.transform.GetChild(
                FrameData.scrollContent.transform.childCount - 1)
                .gameObject;
        var newButton = Instantiate(frameToDuplicate);
        newButton.transform.SetParent(FrameData.scrollContent.transform, false);
        newButton.transform.GetChild(1).GetComponent<Text>().text = "#" +
            (Int32.Parse(frameToDuplicate.transform.GetChild(1)
                 .GetComponent<Text>().text.Substring(1)) + 1);

        FrameData.UpdateBallsInFrame(FrameData.scrollContent.transform
            .childCount - 1);
    }
}
