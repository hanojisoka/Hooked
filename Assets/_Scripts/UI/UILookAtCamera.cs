using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    private UIManager _ui;

    void Start()
    {
        mainCamera = Camera.main;
        _ui = UIManager.Instance;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }

    void OnMouseUp()
    {
        _ui.StartFishReelInButton();
    }
}
