﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FrameData
{
    public static Dictionary<int, Dictionary<int, Vector3>> framePlayerPositions =
        new Dictionary<int, Dictionary<int, Vector3>>();
    public static Dictionary<int, Dictionary<int, Movement>> frameMovements =
        new Dictionary<int, Dictionary<int, Movement>>();
    public static GameObject scrollContent = 
        GameObject.FindWithTag("Stage Preview Content");
    public static int selectedFrame = 0;


    public static void CreateEmptyMovements(int frameNum) {
        frameMovements[frameNum] = new Dictionary<int, Movement>();
        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (var ball in balls) {
            int id = ball.GetComponent<IDable>().id;
            int beats = GetBeatFromFrame(frameNum);
            if (beats == -1) {
                beats = 0;
            }
            frameMovements[frameNum][id] = new LinearMovement(
                new Vector3(ball.transform.position.x,
                            1.0f,
                            ball.transform.position.z),
                new Vector3(ball.transform.position.x,
                            1.0f,
                            ball.transform.position.z),
                beats);
        }
    }

    public static void PushBackFrameData(int frameNum) {
        if ((scrollContent.transform.childCount - 2) <= frameNum) {
            return;
        }

        // Update Movements
        for (int i = scrollContent.transform.childCount - 2;
             i > frameNum; i--) {
            frameMovements[i] = new Dictionary<int, Movement>(
                frameMovements[i - 1]);
        }

        // Update positions
        for (int i = scrollContent.transform.childCount - 1;
             i > frameNum; i--) {
            framePlayerPositions[i] = new Dictionary<int, Vector3>(
                framePlayerPositions[i - 1]);
        }
    }

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
        
        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (var ball in balls) {
            // Add all players to frame
            var playerPrev = GameObject.Instantiate(Resources.Load<Image>(
                "Prefabs/Player Preview"));
            playerPrev.transform.SetParent(frame);
            playerPrev.GetComponent<Image>().color =
                ball.GetComponent<Renderer>().material.color;
            var playerPrevRect = playerPrev.GetComponent<RectTransform>();
            playerPrevRect.localPosition = new Vector3(
                ball.transform.position.x * 10,
                ball.transform.position.z * 10,
                0);
            int ballId = ball.GetComponent<IDable>().id;
            Vector3 ignoringY =  new Vector3(
                ball.transform.position.x,
                1.0f,
                ball.transform.position.z);
            framePlayerPositions[frameNum][ballId] = ignoringY;

            // Update movement
            if (frameNum >= 1) {
                frameMovements[frameNum - 1][ballId].end = ignoringY;
            }
            if (frameNum < scrollContent.transform.childCount - 1) {
                frameMovements[frameNum][ballId].start = ignoringY;
            }
        }
    }

    public static bool SetStageByFrame(int frameNum) {
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

    public static Transform GetFrameTransform(int frameNum) {
        if (scrollContent.transform.childCount <= frameNum) {
            return null;
        }

        return scrollContent.transform.GetChild(frameNum);
    }

    public static int GetBeatFromFrame(int frameNum) {
        if (scrollContent.transform.childCount <= frameNum) {
            return -1;
        }

        string text = GetFrameTransform(frameNum).GetChild(2)
            .GetComponent<InputField>().text;
        if (text == "") {
            return -1;
        }
        return Int32.Parse(text);
    }

    public static void MovePlayers(int frameNumFrom, long ms) {
        int beats = GetBeatFromFrame(frameNumFrom + 1);
        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (var ball in balls) {
            var rb = ball.GetComponent<Rigidbody>();
            int ballId = ball.GetComponent<IDable>().id;
            frameMovements[frameNumFrom][ballId].Move(rb);
            frameMovements[frameNumFrom][ballId].SetPosition(ms, ball);
        }
    }
}
