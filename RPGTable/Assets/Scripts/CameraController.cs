using Cinemachine;
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
    float newZoom;

    [SerializeField] float rotationSpeed = 5.0f;
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] float zoomSpeed = 5.0f;
    

    #endregion

    #region Unity Event Functions

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = Camera.main.fieldOfView;
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
            float a = newZoom - zoomSpeed * Time.deltaTime;

            if (a > 30f)
            {
                newZoom = a;
            }
            else
            {
                newZoom = 30f;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            float a = newZoom + zoomSpeed * Time.deltaTime;

            if (a < 120f)
            {
                newZoom = a;
            }
            else
            {
                newZoom = 120f;
            }
        }

        CheckBounds();
        transform.position = newPosition;
        transform.rotation = newRotation;
        cameraTransform.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = newZoom;

    }

    private void CheckBounds()
    {

        if (newPosition.z < -25f)
        {
            newPosition.z = -25f;
        } 
        else if (newPosition.z > 25f)
        {
            newPosition.z = 25f;
        }

        if (newPosition.x < -25f)
        {
            newPosition.x = -25f;
        }
        else if (newPosition.x > 25f)
        {
            newPosition.x = 25f;
        }
    }
    #endregion
}
