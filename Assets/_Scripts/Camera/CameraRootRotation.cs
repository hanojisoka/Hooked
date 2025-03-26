using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private float rotationY;
    private bool isDragging = false;
    private Vector2 lastTouchPosition;

    void Update()
    {
        // Handle mouse input (PC)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X");
            rotationY += mouseX * rotationSpeed;
        }

        // Handle touch input (Mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.position.x - lastTouchPosition.x;
                rotationY += deltaX * rotationSpeed * 0.02f;
                lastTouchPosition = touch.position;
            }
        }

        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
