//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)
using UnityEngine;

public class PointsPlaneController : MonoBehaviour {
    [SerializeField] protected AudioClip clip = null;

    public void PointsScored() { AudioManager.Instance.GetAudioSources()[1].PlayOneShot(clip); }

    void OnEnable() { S2_PointsPlanesCheckIn.Instance.CheckIn(gameObject); }

    void OnDisable() { S2_PointsPlanesCheckIn.Instance.CheckOut(gameObject); }
}
