using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    AudioClip Pressed;
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Toggle mute;
    [SerializeField]
    Slider[] volumes;

    Text scoreText;
    GameManager instance;
    AudioSource audioSource;

    int score;
    int counter;
    bool isAlive;
    bool s2Start;

    void Awake()
    {
        if (instance != null && instance != this)
            DestroyImmediate(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        mute.isOn = (PlayerPrefs.GetInt("Mute") != 0);
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));

        volumes[0].value = PlayerPrefs.GetFloat("MasterVolume");
        volumes[1].value = PlayerPrefs.GetFloat("MusicVolume");
        volumes[2].value = PlayerPrefs.GetFloat("SFXVolume");
    }

    void Update()
    {
        if (s2Start && SceneManager.GetActiveScene().buildIndex == 2)
        {
            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
            s2Start = false;
        }

        if (scoreText)
            scoreText.text = "Score: " + score.ToString();
    }

    public void TraverseScenes(int unload, int load)
    {
        SceneManager.UnloadSceneAsync(unload);
        SceneManager.LoadScene(load);
    }

    #region Audio Functions
    public void SetMute(bool value)
    {
        AudioListener.pause = value;
        PlayerPrefs.SetInt("Mute", (value ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.Save();
    }
    #endregion

    #region Gamplay Functions
    public bool GetisAlive()
    {
        return isAlive;
    }
    public int GetNumbers(string operation)
    {
        if (operation == "Score")
            return score;
        if (operation == "Counter")
            return counter;
        return 0;
    }
    public void SetNumbers(string operation, int add)
    {
        if (operation == "Score")
            score += add;
        if (operation == "Counter")
            counter++;
    }

    public void ResetGame(string operation)
    {
        if (operation == "Reset")
        {
            s2Start = !s2Start;
            score = 0;
        }
        isAlive = !isAlive;
        counter = 0;
    }
    #endregion

    public void ButtonPressed()
    {
        audioSource.clip = Pressed;
        audioSource.Play();
    }
}