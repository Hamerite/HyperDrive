//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)

using UnityEngine;

public class PointsPlaneController : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    AudioClip clip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PointsScored()
    {
        audioSource.PlayOneShot(clip);
    }
}
