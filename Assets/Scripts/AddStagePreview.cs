using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddStagePreview : MonoBehaviour
{
    [SerializeField]
    private Button previewTemplate;

    private GameObject scrollContent;

    void Awake() {
        scrollContent = GameObject.FindWithTag("Stage Preview Content");
        
        FrameData.UpdateBallsInFrame(0);
    }

    public void OnButtonClick() {
        // Change to selected from to duplicate
        var frameToDuplicate = 
            scrollContent.transform.GetChild(scrollContent.transform.childCount - 1)
                .gameObject;
        var newButton = Instantiate(frameToDuplicate);
        newButton.transform.SetParent(scrollContent.transform, false);
        newButton.transform.GetChild(1).GetComponent<Text>().text = "#" +
            (Int32.Parse(frameToDuplicate.transform.GetChild(1)
                 .GetComponent<Text>().text.Substring(1)) + 1);

        FrameData.UpdateBallsInFrame(scrollContent.transform.childCount - 1);
    }
}
