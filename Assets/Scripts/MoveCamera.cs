using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    private bool moving;
    private bool movingUp;
    private float moveTime;
    private float moveStartTime;
    private bool isUp;

    private Vector3 upPosition = new Vector3(0.0f, 11.0f, 0.0f);
    private Quaternion upRotation = Quaternion.Euler(90, 180, 180);
    private Vector3 frontPosition = new Vector3(0.0f, 9.0f, -9.0f);
    private Quaternion frontRotation = Quaternion.Euler(135, 180, 180);

	// Use this for initialization
	void Start () {
	    movingUp = false;
        moving = false;
        moveTime = 1.0f;
        isUp = false;
	}

    void Update() {
    }

    public void buttonClicked() {
        moveStartTime = Time.fixedTime;
        moving = true;
        movingUp = !isUp;
    }

    void FixedUpdate() {
        if (moving) {
            if (Time.fixedTime - moveStartTime >  moveTime) {
                if (movingUp) {
                    transform.position = upPosition;
                    transform.rotation = upRotation;
                    moving = false;
                    isUp = true;
                } else {
                    transform.position = frontPosition;
                    transform.rotation = frontRotation;
                    moving = false;
                    isUp = false;
                }
            } else {
                transform.Translate(
                    (upPosition - frontPosition) *     
                        (movingUp ? 1.0f : -1.0f) *
                        (Time.fixedTime - moveStartTime)/(25.0f * moveTime),
                    Space.World);
                transform.Rotate(
                    new Vector3(45.0f, 0.0f, 0.0f) *
                        (movingUp ? 1.0f : -1.0f) *
                        (Time.fixedTime - moveStartTime)/(25.0f * moveTime));
            }
        }
    }
}
