using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdateFrame : MonoBehaviour
{
    [SerializeField]
    private GameObject scrollContent;

    void OnMouseUp() {
        var lastFrame = scrollContent.transform.GetChild(
            scrollContent.transform.childCount - 1);

        // Delete all children
        foreach (Transform child in lastFrame.transform) {
            if (child.name.Contains("Player Preview")) {
                Destroy(child.gameObject);
            }
        }

        // Add all players to frame
        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        foreach (var ball in balls) {
            var playerPrev = Instantiate(Resources.Load<Image>("Prefabs/Player Preview"));
            playerPrev.transform.SetParent(lastFrame);
            var playerPrevRect = playerPrev.GetComponent<RectTransform>();
            playerPrevRect.localPosition = new Vector3(
                ball.transform.position.x * 10,
                ball.transform.position.z * 10,
                0); 
        }
    }
}
