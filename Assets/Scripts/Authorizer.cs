using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Authorizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Request Camera permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        // Request Microphone permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        // Initialize ARCore related functionalities
        // Camera feed will be handled automatically by ARCore, no need for manual WebCamTexture setup
    }

    // Update is called once per frame
    void Update()
    {
        // Any AR functionality that you want to update each frame (like object tracking or plane detection)
    }
}