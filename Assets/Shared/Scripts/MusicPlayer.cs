//Created by Dylan LeClair
//Last revised 19-02-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] SongClips;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 4 && !audioSource.isPlaying)
        {
            audioSource.clip = SongClips[Random.Range(0, SongClips.Length)];
            audioSource.Play();
        }
    }

    public void PlaySong(int index)
    {
        audioSource.clip = SongClips[index];
        audioSource.Play();
    }
}
