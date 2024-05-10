using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float moveSpeed = 25.0f;
    private float zoomSpeed;
    private float currentScrollDelta = 55f;
    private float quick = 1f;

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



    private void PanCamera()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * Time.deltaTime * moveSpeed;
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
            quick = 5f;
        }
        else
        {
            quick = 1f;
        }
        if (cam.orthographicSize < 20)
        {
            zoomSpeed = 1;
            moveSpeed = 14 * quick;
        }
        else if (cam.orthographicSize < 100)
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
