//Created by Dylan LeClair 23/09/21
//Last modified 23/09/21 (Dylan LeClair)
using UnityEngine;

public class S5_TrackingCamController : MonoBehaviour {
    public static S5_TrackingCamController Instance { get; private set; }

    [SerializeField] protected GameObject camPos = null, camDefaultPos = null;

    protected bool isInspect;

    void Awake() { Instance = this; }

    void LateUpdate() {
        if (isInspect && Vector3.Distance(Camera.main.transform.position, camPos.transform.position) > 0.01f) Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, camPos.transform.position, 1 * Time.deltaTime);
        else if (!isInspect && Vector3.Distance(Camera.main.transform.position, camDefaultPos.transform.position) > 0.01f) Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, camDefaultPos.transform.position, 1 * Time.deltaTime);
    }

    public void SetIsInspect(bool state) { isInspect = state; }

    public Camera GetCam() { return Camera.main; }
}
