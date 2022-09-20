using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform tableTransform;

    Vector3 camOffset;
    float rotationSpeed = 1.0f;
    float minFov = 20f;
    float maxFov = 70f;
    float zoomSpeed = 10f;

    private void Start()
    {
        camOffset = transform.position - tableTransform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            OrbitCamera();
        }
        UpdateZoom();
    }

    private void UpdateZoom()
    {
        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    private void OrbitCamera()
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);

        transform.position = camOffset = camTurnAngle * camOffset;

        transform.LookAt(tableTransform.position);
    }
}
