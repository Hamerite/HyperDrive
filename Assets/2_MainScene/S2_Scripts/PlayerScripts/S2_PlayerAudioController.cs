//Created by Dylan LeClair
//Last modified 27-09-20 (Dylan LeClair)
using UnityEngine;

public class S2_PlayerAudioController : MonoBehaviour {
    [SerializeField] AudioClip[] clips = null; // { Explosion, PassingObstacle }

    public void PlayAudio(int index) { AudioManager.Instance.GetAudioSources()[1].PlayOneShot(clips[index]); }
}
