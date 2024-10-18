using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARSetup : MonoBehaviour
{
    private ARCameraManager arCameraManager;

    // Start is called before the first frame update
    void Start()
    {
        // Attach ARCameraManager to manage the camera feed
        arCameraManager = FindObjectOfType<ARCameraManager>();

        if (arCameraManager != null)
        {
            Debug.Log("AR Camera Manager found, ARCore will handle the camera feed");
        }
        else
        {
            Debug.LogError("AR Camera Manager not found! Make sure AR Foundation is set up.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // AR functionality can be tracked and updated here
    }
}
