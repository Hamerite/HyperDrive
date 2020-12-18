//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)

using UnityEngine;

public class PointsPlaneController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] AudioClip clip = null;

    public void PointsScored()
    {
        audioSource.PlayOneShot(clip);
    }
}
