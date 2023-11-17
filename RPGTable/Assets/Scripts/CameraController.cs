using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    #region Variables

    [SerializeField] Transform cameraTransform;

    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;

    [SerializeField] float rotationSpeed = 5.0f;
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] Vector3 zoomSpeed = new Vector3(0,-1.0f,1.0f);



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
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        HandleMovementInput();
    }

    #endregion

    #region Camera Related Methods

    private void HandleMovementInput()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            newPosition += transform.forward * movementSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
        {
            newPosition -= transform.right * movementSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            newPosition -= transform.forward * movementSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            newPosition += transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed * Time.deltaTime);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            newZoom += zoomSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            newZoom -= zoomSpeed * Time.deltaTime;
        }

        if (newPosition != transform.position && !OutOfBounds())
        {
            transform.position = newPosition;
        }
        
        transform.rotation = newRotation;
        cameraTransform.localPosition = newZoom;

    }

    private bool OutOfBounds()
    {
        return newPosition.z < -22.5f || newPosition.z > 22.5f || newPosition.x < -22.5f || newPosition.x > 22.5f; 
    }

    #endregion
}
