//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerAudioController : MonoBehaviour {
    public static S2_PlayerAudioController Instance { get; private set; }

    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected AudioClip[] clips = null; // { Explosion, Whoosh(1-5), MetalScraping }

    void Awake() { Instance = this; }

    public void PlayAudio(int index) { audioSource.PlayOneShot(clips[index]); }

    public void PlayRandomWhoosh() { audioSource.PlayOneShot(clips[Random.Range(1, 4)]); }
}
