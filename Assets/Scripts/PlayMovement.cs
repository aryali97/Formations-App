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
        PlayerSelect.UnselectAll();

        FrameData.SetStageByFrame(0);
        playing = true; 
        framePlayingFrom = 0;
        UnityEngine.Debug.Log("Playing");
    }

    void FixedUpdate() {
        if (!playing) {
            return;
        }
        if (stopWatch == null || !stopWatch.IsRunning) {
            stopWatch = Stopwatch.StartNew();
        }

        if (framePlayingFrom >= FrameData.scrollContent.transform.childCount - 1) {
            playing = false;
            framePlayingFrom = 0;
            stopWatch.Stop();
            UnityEngine.Debug.Log("Stopped because reached last frame");
            return;
        }

        FrameData.MovePlayers(framePlayingFrom, stopWatch.ElapsedMilliseconds);
        if ((stopWatch.ElapsedMilliseconds)  >
            (FrameData.GetBeatFromFrame(framePlayingFrom + 1) * 1000)) {
            FrameData.SetStageByFrame(framePlayingFrom + 1);
            framePlayingFrom++;
            stopWatch = Stopwatch.StartNew();
            return;
        }         
    }

    bool AllFramesHaveBeats() {
        bool allFramesHaveBeats = true;
        for (int i = 1; i < FrameData.scrollContent.transform.childCount; i++) {
            if (FrameData.GetBeatFromFrame(i) < 0) {
                UnityEngine.Debug.Log("Frame " + i + " doesn't have data");
                allFramesHaveBeats = false;
                break;
            }
        }
        return allFramesHaveBeats;
    }
}
