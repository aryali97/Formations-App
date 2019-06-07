using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    public Vector3 start;
    public Vector3 end;
    public int beats;

    public Movement(Vector3 startVec, Vector3 endVec, int b) {
        start = startVec;
        end = endVec;
        beats = b;
    }

    public abstract void Move(Rigidbody rb);
}
