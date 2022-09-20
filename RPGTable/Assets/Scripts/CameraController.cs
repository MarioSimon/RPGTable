using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform tableTransform;

    Vector3 camOffset;
    float rotationSpeed = 1.0f;

    private void Start()
    {
        camOffset = transform.position - tableTransform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            RotateCamera();
        }
    }

    void RotateCamera()
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);

        transform.position = camOffset = camTurnAngle * camOffset;

        transform.LookAt(tableTransform.position);
    }
}
