using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddStagePreview : MonoBehaviour
{
    [SerializeField]
    private Button previewTemplate;

    void Awake() {
    }

    public void OnButtonClick() {
        var newButton = Instantiate(previewTemplate);
        newButton.transform.SetParent(previewTemplate.transform.parent, false);

        var balls = GameObject.FindGameObjectsWithTag("Player Ball");
        Debug.Log("Balls length: " + balls.Length);
        foreach (var ball in balls) {
            var playerPrev = Instantiate(Resources.Load<Image>("Prefabs/Player Preview"));
            playerPrev.transform.SetParent(newButton.transform);

            var playerPrevRect = playerPrev.GetComponent<RectTransform>();
            playerPrevRect.localPosition = new Vector3(
                ball.transform.position.x * 10,
                ball.transform.position.z * 10,
                0);
            Debug.Log("Player pos: " + playerPrevRect.localPosition);
            /*
            playerPrev.transform.position = new Vector3(
                ball.transform.position.x * 10,
                ball.transform.position.z * 10,
                0);
            Debug.Log("Player pos: " + playerPrev.transform.position);
            */
        }
    }
}
