using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables

    [SerializeField] Transform cameraTransform;

    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;

    [SerializeField] float rotationSpeed = 1.0f;
    [SerializeField] float movementSpeed = 0.1f;
    [SerializeField] Vector3 zoomSpeed = new Vector3(0,-0.5f,0.5f);



    #endregion

    #region Unity Event Functions

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        HandleMovementInput();
        //UpdateZoom();
    }

    #endregion

    #region Camera Related Methods

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition -= transform.right * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition -= transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            newZoom += zoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            newZoom -= zoomSpeed;
        }

        transform.position = newPosition;
        transform.rotation = newRotation;
        cameraTransform.localPosition = newZoom;

    }

    #endregion
}
