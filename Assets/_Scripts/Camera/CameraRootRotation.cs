using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootRotation : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    public float rotationSpeed = 5f;
    private float rotationY;
    private bool isDragging = false;
    private Vector2 lastTouchPosition;
    private bool _canRotate = true;

    void Update()
    {
        // if settings panel is active, don't rotate the camera
        if(_settingsPanel.activeSelf) return;

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
