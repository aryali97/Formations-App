using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTestScript : MonoBehaviour {

    public float speed;
    public GameObject stage;

    private Rigidbody rb;
    private float stageXMin;
    private float stageXMax;
    private float stageZMin;
    private float stageZMax;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        stageXMin = 
            (stage.transform.position.x - stage.transform.localScale.x/2.0f);
        stageXMax = 
            (stage.transform.position.x + stage.transform.localScale.x/2.0f);
        stageZMin = 
            (stage.transform.position.z - stage.transform.localScale.z/2.0f);
        stageZMax = 
            (stage.transform.position.z + stage.transform.localScale.z/2.0f);
        Debug.Log("X min max: (" + stageXMin + ", " + stageXMax + ")");
        Debug.Log("Z min max: (" + stageZMin + ", " + stageZMax + ")");
	}

    // Clamp position to stage size
    void OnMouseDrag() {
        var posit = this.transform.position;
        posit.x = Mathf.Clamp(posit.x, stageXMin, stageXMax);
        posit.z = Mathf.Clamp(posit.z, stageZMin, stageZMax);
        this.transform.position = posit;
    }

    // Delete velocity/force when let go
    void OnMouseUp() {
        rb.velocity = Vector3.zero;
    }

    // Called right before physics
    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // TODO: Should remove x,z force probably
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }
}
