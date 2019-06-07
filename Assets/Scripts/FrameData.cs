using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FrameData
{
    public static Dictionary<int, Dictionary<int, Vector3>> framePlayerPositions =
        new Dictionary<int, Dictionary<int, Vector3>>();
    public static GameObject scrollContent = 
        GameObject.FindWithTag("Stage Preview Content");


    public static void UpdateBallsInFrame(int frameNum) {
        var frame = scrollContent.transform.GetChild(frameNum);

        if (!framePlayerPositions.ContainsKey(frameNum)) {
            framePlayerPositions[frameNum] = new Dictionary<int, Vector3>();
        }

        // Delete all children
        foreach (Transform child in frame.transform) {
            if (child.name.Contains("Player Preview")) {
                GameObject.Destroy(child.gameObject);
            }
        }

        // Add all players to frame
        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (var ball in balls) {
            var playerPrev = GameObject.Instantiate(Resources.Load<Image>(
                "Prefabs/Player Preview"));
            playerPrev.transform.SetParent(frame);
            var playerPrevRect = playerPrev.GetComponent<RectTransform>();
            playerPrevRect.localPosition = new Vector3(
                ball.transform.position.x * 10,
                ball.transform.position.z * 10,
                0);
            framePlayerPositions[frameNum][ball.GetComponent<IDable>().id] =
                new Vector3(
                    ball.transform.position.x,
                    1.0f,
                    ball.transform.position.z
                );
        }
    }

    public static bool SetStageByFrame(int frameNum) {
        Debug.Log("Set stage by frame called");
        if (!framePlayerPositions.ContainsKey(frameNum)) {
            return false;
        }

        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (var ball in balls) {
            ball.transform.position = framePlayerPositions[frameNum]
                [ball.GetComponent<IDable>().id];
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        return true;
    }

    public static int GetBeatFromFrame(int frameNum) {
        if (scrollContent.transform.childCount <= frameNum) {
            return -1;
        }

        foreach (Transform inputFieldChild in scrollContent.transform.GetChild(
            frameNum).GetChild(2)) {
            if (inputFieldChild.name.Contains("Text")) {
                string text = inputFieldChild.GetComponent<Text>().text;
                if (text == "") {
                    return -1;
                }
                return Int32.Parse(text);
            }
        }
        return -1;
    }

    public static void MovePlayers(int frameNumFrom) {
        int beats = GetBeatFromFrame(frameNumFrom + 1);
        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (var ball in balls) {
            var rb = ball.GetComponent<Rigidbody>();
            int ballId = ball.GetComponent<IDable>().id;
            rb.velocity = (framePlayerPositions[frameNumFrom + 1][ballId] -
                framePlayerPositions[frameNumFrom][ballId]) / ((float)beats);
            rb.angularVelocity = Vector3.zero;
        }
    }
}
