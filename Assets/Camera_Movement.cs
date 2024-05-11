using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    private float moveSpeed = 25.0f;
    private float zoomSpeed;
    private float currentScrollDelta = 60f;
    private float quick = 1f;

    private Vector3 origin;
    private Vector3 difference;

    private bool drag = false;

    private Camera cam;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        PanCamera();
        ZoomCamera();
        ChangeCamSpeed();
    }

    private void LateUpdate()
    {
        DragCamera();
    }


    private void DragCamera()
    {
        if (Input.GetMouseButton(2))
        {
            difference = (cam.ScreenToWorldPoint(Input.mousePosition)) - cam.transform.position;
            if (drag == false)
            {
                drag = true;
                origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            cam.transform.position = origin - difference;
        }
    }

    private void PanCamera()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Time.unscaledDeltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * Time.unscaledDeltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Time.unscaledDeltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * Time.unscaledDeltaTime * moveSpeed;
        }
    }

    private void ZoomCamera()
    {
        if (cam.orthographicSize > 1)
        {
            currentScrollDelta += Input.mouseScrollDelta.y * -1 * zoomSpeed;
            cam.orthographicSize = currentScrollDelta;

            if (Input.GetKeyDown(KeyCode.Equals))
            {
                cam.orthographicSize -= 2 * zoomSpeed;
                currentScrollDelta = cam.orthographicSize;
            } else if (Input.GetKeyDown(KeyCode.Minus))
            {
                cam.orthographicSize += 2 * zoomSpeed;
                currentScrollDelta = cam.orthographicSize;
            }
        } 
        else if (cam.orthographicSize < 2)
        {
            cam.orthographicSize = 2;
            currentScrollDelta = 2;
        }
    }

    private void ChangeCamSpeed()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            quick = 4f;
        }
        else
        {
            quick = 1f;
        }
        if (cam.orthographicSize < 20)
        {
            zoomSpeed = 1;
            moveSpeed = 8f * quick;
        }
        else if (cam.orthographicSize < 60)
        {
            zoomSpeed = 2 * quick;
            moveSpeed = 25 * quick;
        }
        else if (cam.orthographicSize < 1000)
        {
            zoomSpeed = 4 * quick;
            moveSpeed = 50 * quick;
        }
    }
}
