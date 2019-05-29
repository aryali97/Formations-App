using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTestScript : MonoBehaviour {

    public float speed;
    public GameObject stage;
    //public Toggle snapToggle;
    //public float snapDist;

    private Rigidbody rb;
    //private List<float> vertLines = new List<float>();
    //private List<Vector2> lines = new List<Vector2>();

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        /*
        for (int i = -2; i <= 2; ++i) {
            vertLines.Add(i * 2.0f);
        }
        for (int i = -1; i <= 1; ++i) {
            lines.Add(new Vector2(0.0f, 2.0f * i));
        }
        lines.Add(new Vector2(0.5f, 0.0f));
        */
	}

    /*
    void OnMouseDrag() {
        Plane plane = new Plane(Vector3.up, new Vector3(0, 1.2f, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance)) {
            var newPos = ray.GetPoint(distance);
            if (snapToggle.isOn) {
                var snappedCoords = SnapToLines(newPos.x, newPos.z);
                transform.position = new Vector3(
                    snappedCoords.x,
                    newPos.y, 
                    snappedCoords.y);
            } else {
                transform.position = newPos;
            }
            rb.velocity = Vector3.zero;
        }
    }
    */

    // Called right before physics
    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    /*
    struct isVertAndLineIndex {
        public bool isVert;
        public int lineIndex;

        public isVertAndLineIndex(bool b, int i) {
            isVert = b;
            lineIndex = i;
        }
    }
    Vector2 SnapToLines(float x, float z) {
        // float xBestDist = snapDist;
        // float yBestDist = snapDist;
        float bestDist = snapDist;
        float newX = x, newZ = z;
        List<isVertAndLineIndex> isVertAndLine = new List<isVertAndLineIndex>();
        for (int i = 0; i < vertLines.Count; ++i) {
            float vert = vertLines[i];
            if (Math.Abs(x - vert) < bestDist) {
                bestDist = Math.Abs(x - vert);
                newX = vert;
                newZ = z;
                isVertAndLine.Add(new isVertAndLineIndex(true, i));
            }
        }
        bestDist = snapDist;
        for (int i = 0; i < lines.Count; ++i) {
            var tup = lines[i];
            if (tup.x == 0.0f && Math.Abs(z - tup.y) < bestDist) {
                bestDist = Math.Abs(z - tup.y);
                newX = x;
                newZ = tup.y;
                isVertAndLine.Add(new isVertAndLineIndex(false, i));
            } else if (tup.x != 0.0f) {
                // Y - y = m (X - x) 
                // Y = M (X - x) + y
                // Y1 = m1X1 + b1
                // m1*X + b1 = m2(X - x) + y
                // X(m1 - m2) = y - m2x - b1
                // X = (y - m2x - b1)/(m1 - m2)
                float m1 = tup.x;
                float m2 = -1.0f/tup.x;
                float b = tup.y;
                float posX = (z - m2 * x - b)/(m1 - m2);
                float posY = m1 * posX + b;
                float dist = (float)Math.Sqrt(Math.Pow(posX - x, 2) + Math.Pow(posY - z, 2));
                if (dist < bestDist) {
                    bestDist = dist;
                    newX = posX;
                    newZ = posY;
                    isVertAndLine.Add(new isVertAndLineIndex(false, i));
                }
            }
        }
        bestDist = snapDist;
        for (int i = 0; i < isVertAndLine.Count; ++i) {
            for (int j = 0; j < isVertAndLine.Count; ++j) {
                if (isVertAndLine[i].isVert && isVertAndLine[j].isVert) {
                    continue;
                } else if (!isVertAndLine[i].isVert && !isVertAndLine[j].isVert) {
                    if (lines[isVertAndLine[i].lineIndex].x == lines[isVertAndLine[i].lineIndex].x) {
                        continue;
                    }
                    // Y = M1*X + B1
                    // Y = M2*X + B2
                    // m1*X + b1 = m2*X + b2
                    // X(m1 - m2) = b2 - b1
                    // X = (b2 - b1)/(m1 - m2)
                    var line1 = lines[isVertAndLine[i].lineIndex];
                    var line2 = lines[isVertAndLine[j].lineIndex];
                    float posX = (line2.y - line1.y)/(line1.x - line2.x);
                    float posY = line1.x * posX + line1.y;
                    float dist = (float)Math.Sqrt(Math.Pow(posX - x, 2) + Math.Pow(posY - z, 2));
                    if (dist < bestDist) {
                        bestDist = dist;
                        newX = posX;
                        newZ = posY;
                    }
                } else {
                    float posX = 0.0f, posY = 0.0f;
                    if (isVertAndLine[i].isVert) {
                        posX = vertLines[isVertAndLine[i].lineIndex];
                        posY = posX * lines[isVertAndLine[j].lineIndex].x +
                            lines[isVertAndLine[j].lineIndex].y;
                    } else {
                        posX = vertLines[isVertAndLine[j].lineIndex];
                        posY = posX * lines[isVertAndLine[i].lineIndex].x +
                            lines[isVertAndLine[i].lineIndex].y;
                    }
                    float dist = (float)Math.Sqrt(Math.Pow(posX - x, 2) + Math.Pow(posY - z, 2));
                    if (dist < bestDist) {
                        bestDist = dist;
                        newX = posX;
                        newZ = posY;
                    }
                }
            }
        }
        return new Vector2(newX, newZ);
    }
    */
	
	// Update is called once per frame
	void Update () {
        float x = transform.position.x;
        float z = transform.position.z;
        if (x < (stage.transform.position.x - stage.transform.localScale.x/2.0f)) {
            x = (stage.transform.position.x - stage.transform.localScale.x/2.0f);
        } else if (x > (stage.transform.position.x + stage.transform.localScale.x/2.0f)) {
            x = (stage.transform.position.x + stage.transform.localScale.x/2.0f);
        }
        if (z < (stage.transform.position.z - stage.transform.localScale.z/2.0f)) {
            z = (stage.transform.position.z - stage.transform.localScale.z/2.0f);
        } else if (z > (stage.transform.position.z + stage.transform.localScale.z/2.0f)) {
            z = (stage.transform.position.z + stage.transform.localScale.z/2.0f);
        }
	}
}
