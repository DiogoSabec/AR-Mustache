using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARCameraManager))]
public class SwitchToFrontCamera : MonoBehaviour
{
    private ARCameraManager cameraManager;

    void Start()
    {
        // Get the ARCameraManager component attached to the camera
        cameraManager = GetComponent<ARCameraManager>();

        if (cameraManager != null)
        {
            // Switch to the front-facing camera (User camera)
            cameraManager.requestedFacingDirection = CameraFacingDirection.User;
        }
        else
        {
            Debug.LogWarning("ARCameraManager component is missing.");
        }
    }
}
    