﻿//Created by Dylan LeClair
//Last revised 19-02-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {
    [SerializeField] protected AudioClip[] SongClips = null;

    void Update() {
        if (SceneManager.GetActiveScene().name == "Music" || AudioManager.Instance.GetAudioSources()[0].isPlaying) return;

        AudioManager.Instance.GetAudioSources()[0].clip = SongClips[Random.Range(0, SongClips.Length)];
        AudioManager.Instance.GetAudioSources()[0].Play();
    }

    public void PlaySong(int index) {
        AudioManager.Instance.GetAudioSources()[0].clip = SongClips[index];
        AudioManager.Instance.GetAudioSources()[0].Play();
    }
}
