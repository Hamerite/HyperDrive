//Created by Kyle Ennis 15/09/21
//Last modified 15/09/21 (Dylan LeClair)
using System.Collections;
using UnityEngine;

[AddComponentMenu("Camera/Camera Path", 1)]
public class CameraPaths : MonoBehaviour {
    [Header("Camera Path References")]
    [SerializeField] protected CPC_CameraPath path;
    [SerializeField] protected CPC_CameraPath endCam;
    [SerializeField] protected CPC_CameraPath[] paths;

    [Header("Behaviour Variables")]
    [SerializeField] protected bool playOnStart;
    [SerializeField] protected float time, startDelay, stayTime, endCamStayTime;

    protected WaitForEndOfFrame waitForEndOfFrame;
    protected bool hasPlayed = false;

    void Start() {
        paths = FindObjectsOfType<CPC_CameraPath>();
        if (playOnStart) Invoke(nameof(StartPathing), startDelay);
    }

    public void StartPathing() {
        for (int i = 0; i < paths.Length - 1; i++) { paths[i].gameObject.SetActive(false); }
        path.gameObject.SetActive(true);

        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence() {
        yield return waitForEndOfFrame;

        if (endCam != null) { path.PlayPath(time, stayTime, endCamStayTime, endCam); }
        else { path.PlayPath(time, stayTime, endCamStayTime, null); }
    }
}
