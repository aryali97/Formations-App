using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldBeatUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<InputField>().onValueChanged.AddListener(
            delegate {UpdateBeats();});
    }

    void UpdateBeats()
    {
        if (transform.parent.GetSiblingIndex() == 0) {
            return;
        }
        int frameNum = gameObject.transform.parent.GetSiblingIndex();
        var inputField = GetComponent<InputField>();
        int beats = (inputField.text == "") ? 0 : Int32.Parse(inputField.text);
        foreach (int ballId in FrameData.frameMovements[frameNum - 1].Keys) {
            FrameData.frameMovements[frameNum - 1][ballId].beats = beats;
        }
    }
}
