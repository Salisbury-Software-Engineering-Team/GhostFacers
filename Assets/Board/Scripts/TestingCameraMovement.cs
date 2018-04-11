using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCameraMovement : MonoBehaviour {

    public float Speed = 20;

    private void OnMouseDrag()
    {
        float x = Input.GetAxis("Mouse X") * Speed * Mathf.Deg2Rad;
        float y = Input.GetAxis("Mouse Y") * Speed * Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -x); 
        transform.RotateAround(Vector3.right, y);
    }
}
