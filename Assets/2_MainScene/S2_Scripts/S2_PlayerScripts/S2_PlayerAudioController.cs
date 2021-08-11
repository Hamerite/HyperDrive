//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerAudioController : MonoBehaviour {
    public static S2_PlayerAudioController Instance { get; private set; }

    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected AudioClip[] clips = null; // { Explosion, PassingObstacle }

    void Awake() { Instance = this; }

    public void PlayAudio(int index) { audioSource.PlayOneShot(clips[index]); }
}
