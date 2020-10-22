//Created by Dylan LeClair
//Last revised 19-02-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] SongClips;

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
