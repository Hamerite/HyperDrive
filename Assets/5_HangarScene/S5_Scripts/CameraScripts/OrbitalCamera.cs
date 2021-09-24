//Created by Kyle Ennis 13/09/21
//Last modified 22/09/21 (Dylan LeClair)
using UnityEngine;

public class OrbitalCamera : MonoBehaviour {
    public static OrbitalCamera Instance { private set; get; }

    [SerializeField] GameObject trackingPosition;
   
    [SerializeField] bool InvertCamY;
    [SerializeField] [Range(1, 10)] float sensitivity = 2.5f;
    [SerializeField] [Range(0, 1)] float rotationsmoothTime = 0.202f;

    Vector2 pitchMinMax = new Vector2(-70,14);
    Vector3 smoothingVelocity, currentRotation;

    float YAxis, pitch, yaw, cameraOffsetY = 0.3f, camDist = -2;

    void Awake() { Instance = this; }

    void LateUpdate() { CamMovement3D(); }

    void CamMovement3D() {
        if (Input.GetMouseButton(0)) {
            YAxis = Input.GetAxis("Mouse Y") * sensitivity;
            yaw += Input.GetAxis("Mouse X") * sensitivity;

            pitch = (InvertCamY) ? pitch += YAxis : pitch -= YAxis;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        }

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref smoothingVelocity, rotationsmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = trackingPosition.transform.position - (transform.forward - (transform.right) + (transform.up * cameraOffsetY)) * camDist;
        S5_TrackingCamController.Instance.GetCam().transform.LookAt(trackingPosition.transform.position);
    }

    //needs to be tied to menu system for adjustment by player
    public void switchInverseY() { InvertCamY = !InvertCamY; }
}
