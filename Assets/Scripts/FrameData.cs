﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FrameData
{
    public static Dictionary<int, Dictionary<int, Vector3>> framePlayerPositions =
        new Dictionary<int, Dictionary<int, Vector3>>();


    public static void UpdateBallsInFrame(int frameNum) {
        var scrollContent = GameObject.FindWithTag("Stage Preview Content");
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
                ball.transform.position;
        }
    }
}