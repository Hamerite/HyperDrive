//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioClip[] buttonSoundClips; // { MousedOver, Pressed, saveHighScore, deleteHighScore, DenySelection, Coins }

    void Start()
    {
        Instance = this;

        if (PlayerPrefs.GetInt("Mute") != 0)
            AudioListener.pause = true;
        audioMixer.SetFloat("MixerMaster", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        audioMixer.SetFloat("MixerMusic", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("MixerSFX", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
    }

    public void SetMute(bool value)
    {
        AudioListener.pause = value;
        PlayerPrefs.SetInt("Mute", (value ? 1 : 0));
        SaveSettings();
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MixerMaster", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
        SaveSettings();
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MixerMusic", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        SaveSettings();
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("MixerSFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
        SaveSettings();
    }

    void SaveSettings()
    {
        PlayerPrefs.Save();
        if (Cursor.visible)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void PlayButtonSound(int index)
    {
        audioSource.PlayOneShot(buttonSoundClips[index]);
    }

    public void PlayLoopingAudio(int index)
    {
        audioSource.loop = true;
        audioSource.clip = buttonSoundClips[index];
        audioSource.Play();
    }

    public void StopLoopingAudio()
    {
        audioSource.loop = false;
        audioSource.clip = null;
        audioSource.Stop();
    }
}
