using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public static OrbitalCamera Instance { private set; get; }
    float YAxis, pitch, yaw, cameraOffsetY = 0.3f, camDist = -2;
   
    Vector2 pitchMinMax = new Vector2(-70,14);
    Vector3 smoothingVelocity, currentRotation;

    [Header("Invert Camera Y")]
    [SerializeField] bool invY;

    [Header("Camera Sensitivity and rotation smoothing")]
    [Tooltip("Adjusts camera movement sensitivity.")]
    [Range(1, 10)]
    [SerializeField] float sensitivity = 2.5f;

    [Tooltip("Adjusts how quickly the camera moves acording to user input.")]
    [Range(0, 1)]
    [SerializeField] float rotationsmoothTime = 0.202f;

    [SerializeField] GameObject trackingPosition;

    private void Awake()
    {
        Instance = this;
    }

    void CamMovement3D()
    {
        if (Input.GetMouseButton(0))
        {
            YAxis = Input.GetAxis("Mouse Y") * sensitivity;
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch = (invY) ? pitch += YAxis : pitch -= YAxis;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        }

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref smoothingVelocity, rotationsmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = trackingPosition.transform.position - (transform.forward - (transform.right) + (transform.up * cameraOffsetY)) * camDist;
        HangarScript.Instance.GetCam().transform.LookAt(trackingPosition.transform.position);
    }
    private void LateUpdate()
    {
        CamMovement3D();
    }

    //needs to be tied to menu system for adjustment by player
    public void switchInverseY()
    {
        invY = !invY;
    }
}
