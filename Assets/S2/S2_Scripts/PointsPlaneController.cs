//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)

using UnityEngine;

public class PointsPlaneController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    public void PointsScored()
    {
        audioSource.PlayOneShot(clip);
    }
}
