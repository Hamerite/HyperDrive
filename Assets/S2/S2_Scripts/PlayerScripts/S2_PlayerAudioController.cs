//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)

using UnityEngine;

public class S2_PlayerAudioController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips; // { Explosion, PassingObstacle }

    public void PlayAudio(int index)
    {
        audioSource.PlayOneShot(clips[index]);
    }
}
