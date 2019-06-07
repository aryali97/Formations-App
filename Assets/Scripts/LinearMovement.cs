using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : Movement
{
    public LinearMovement(Vector3 startVec, Vector3 endVec, int b)
        : base(startVec, endVec, b) {}

    public override void Move(Rigidbody rb) {
        rb.velocity = (end - start) / ((float)beats); 
        rb.angularVelocity = Vector3.zero;
    }
}
