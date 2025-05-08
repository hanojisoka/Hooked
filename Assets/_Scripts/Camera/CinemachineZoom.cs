using UnityEngine;
using Cinemachine;

public class CinemachineZoom : MonoBehaviour
{
    public CinemachineVirtualCamera zoomOutCam; // Default (Zoomed-out) Camera
    public CinemachineVirtualCamera zoomInCam;  // Zoomed-in Camera


    public Transform zoomInPoint;  // The closest zoom position
    public Transform zoomOutPoint; // The farthest zoom position
    public float zoomSpeed = 5f;   // Speed of zoom transition
    private float zoomLerp = 0.5f; // Lerp value (0 = close, 1 = far)

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     zoomOutCam.gameObject.SetActive(!zoomOutCam.gameObject.activeSelf);
        //     zoomInCam.gameObject.SetActive(!zoomInCam.gameObject.activeSelf);
        // }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            zoomLerp -= scroll * zoomSpeed * Time.deltaTime;
            zoomLerp = Mathf.Clamp01(zoomLerp); // Keep between 0 and 1

            // Smoothly move the camera between the zoom points
            zoomInCam.transform.position = Vector3.Lerp(zoomInPoint.position, zoomOutPoint.position, zoomLerp);
            zoomInCam.transform.transform.rotation = Quaternion.Slerp(zoomInPoint.rotation, zoomOutPoint.rotation, zoomLerp);
            
            if(zoomInCam.gameObject.activeSelf && zoomInCam.transform.position == zoomOutPoint.position) // if zoomIn camera is active and if zoom cam is in zoomOutPoint pos
            {
                Debug.Log("Max Zoom Out Reached");
                zoomOutCam.gameObject.SetActive(true);
                zoomInCam.gameObject.SetActive(false);
            }
            if (zoomOutCam.gameObject.activeSelf && scroll > 0)
            {
                Debug.Log("Switching to ZoomIn Cam");
                zoomOutCam.gameObject.SetActive(false);
                zoomInCam.gameObject.SetActive(true);
            }
        }
    }
}

