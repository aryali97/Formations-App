using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlayMovement : MonoBehaviour
{

    private bool playing;
    private int framePlayingFrom;
    private Stopwatch stopWatch;

    void Awake() {
        playing = false;
    }

    public void PlayOnClick() {
        // Check that all frames have numbers
        if (!AllFramesHaveBeats()) {
            UnityEngine.Debug.Log("ERROR: Can't play, not all frames have " +
                "beats");
            return;
        }

        playing = true; 
        framePlayingFrom = 0;
        stopWatch = Stopwatch.StartNew();
        UnityEngine.Debug.Log("Playing");
    }

    void Update() {
        if (!playing) {
            return;
        }
        if (framePlayingFrom >= FrameData.scrollContent.transform.childCount - 1) {
            playing = false;
            framePlayingFrom = 0;
            stopWatch.Stop();
            UnityEngine.Debug.Log("Stopped because reached last frame");
            return;
        }
        if (stopWatch.ElapsedMilliseconds > 2000) {
            UnityEngine.Debug.Log("2 seconds finished!");
            playing = false;
            framePlayingFrom = 0;
            stopWatch.Stop();
            return;
        }
    }

    bool AllFramesHaveBeats() {
        bool allFramesHaveBeats = true;
        for (int i = 1; i < FrameData.scrollContent.transform.childCount; i++) {
            Transform inputField = FrameData.scrollContent.transform.GetChild(i)
                .GetChild(2);
            foreach (Transform inputFieldChild in inputField) {
                if (inputFieldChild.name.Contains("Text")) {
                    if (inputFieldChild.GetComponent<Text>().text == "") {
                        allFramesHaveBeats = false;
                    }
                    break;
                }
            }
            if (!allFramesHaveBeats) {
                break;
            }
        }
        return allFramesHaveBeats;
    }
}
