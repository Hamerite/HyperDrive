using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_ButtonController : MonoBehaviour
{
    [SerializeField]
    AudioClip musicSelect;

    AudioSource audioSource;
    GameManager gameManager;
    MusicPlayer musicPlayer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        musicPlayer = FindObjectOfType<MusicPlayer>();

        audioSource.clip = musicSelect;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void ButtonSelected()
    {
        audioSource.Play();
    }

    #region Song Choice
    public void PlayActionable()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(0);
    }

    public void PlayBirthOfAHero()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(1);
    }

    public void PlayDubStep()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(2);
    }

    public void PlayEndlessMotion()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(3);
    }

    public void PlayEvolution()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(4);
    }

    public void PlayExtremeAction()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(5);
    }

    public void PlayHighOctane()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(6);
    }

    public void PlayNewDawn()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(7);
    }

    public void PlaySlowMotion()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(8);
    }

    #endregion

    public void MainMenuButton()
    {
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(4, 1);
    }
}
