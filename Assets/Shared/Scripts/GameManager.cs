﻿//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region Audio Cache Variables
    [SerializeField]
    AudioMixer audioMixer;

    AudioSource audioSource;
    [SerializeField]
    AudioClip[] buttonSoundClips; // { MousedOver, Pressed, saveHighScore, deleteHighScore, DenySelection }
    
    [SerializeField]
    Toggle mute;
    #endregion

    #region Gameplay Variables
    Text scoreText;
    Text obstaclesPassed;
    Text levelReached;
    int score;
    int counter;
    int coins;

    string levelText;

    bool s2Start;
    #endregion

    #region Menu Navigation Variables
    Vector3 mousePos;

    bool usingKeys;
    #endregion

    void Awake()
    {
        if (Instance != null && Instance != this)
            DestroyImmediate(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        mute.isOn = (PlayerPrefs.GetInt("Mute") != 0);
        audioMixer.SetFloat("MixerMaster", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        audioMixer.SetFloat("MixerMusic", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("MixerSFX", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);

        coins = PlayerPrefs.GetInt("Coins");

        mousePos = Input.mousePosition;
    }

    void Update()
    {
        if(s2Start && SceneManager.GetActiveScene().buildIndex == 2)
        {
            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
            obstaclesPassed = GameObject.FindGameObjectWithTag("ObstaclesPassed").GetComponent<Text>();
            levelReached = GameObject.FindGameObjectWithTag("LevelReached").GetComponent<Text>();
            s2Start = false;
        }
        if (scoreText)
            scoreText.text = "Score: " + score.ToString();
        if (obstaclesPassed)
            obstaclesPassed.text = "Obstacle: " + counter.ToString();
        if (levelReached)
            levelReached.text = "Level: " + levelText;

        if (SceneManager.GetActiveScene().buildIndex == 0)
            MouseToKeys(null, null);
        KeysToMouse();
    }

    #region Gamplay Functions
    public int GetNumbers(string operation)
    {
        if (operation == "Score")
            return score;
        if (operation == "Counter")
            return counter;
        if (operation == "Coins")
            return coins;
        return 0;
    }

    public void SetNumbers(string operation, int add)
    {
        if (operation == "Score")
            score += add;
        if (operation == "Counter")
            counter++;
        if (operation == "Coins")
        {
            coins += (score * 5) + (counter * 2);
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();
        }
    }

    public void SetLevel(string level)
    {
        levelText = level;
    }
    #endregion

    #region Audio Functions
    public void SetMute(bool value)
    {
        AudioListener.pause = value;
        PlayerPrefs.SetInt("Mute", (value ? 1 : 0));
        PlayerPrefs.Save();

        if(Cursor.visible)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MixerMaster", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

        if (Cursor.visible)
            EventSystem.current.SetSelectedGameObject(null);
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MixerMusic", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        if (Cursor.visible)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("MixerSFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();

        if (Cursor.visible)
            EventSystem.current.SetSelectedGameObject(null);
    }
    #endregion

    #region Menu Navigation Functions
    public void MouseToKeys(Button newFirstSelectedButton, Toggle newFirstSelectedToggle)
    {
        if (!usingKeys && Input.GetAxis("Vertical") != 0 || !usingKeys && Input.GetAxis("Horizontal") != 0)
        {
            mousePos = Input.mousePosition;

            usingKeys = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                if (newFirstSelectedToggle)
                    newFirstSelectedToggle.Select();
                else
                    newFirstSelectedButton.Select();
            }
        }
    }

    void KeysToMouse()
    {
        if (usingKeys && mousePos != Input.mousePosition)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            usingKeys = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    #endregion

    #region Scenes Shared Fuctions
    public void TraverseScenes(int unload, int load)
    {
        SceneManager.LoadScene(load);
        SceneManager.UnloadSceneAsync(unload);

        if (load == 2)
        {
            s2Start = true;
            score = 0;
            counter = 0;
        }
    }

    public void PlayButtonSound(int index)
    {
        audioSource.PlayOneShot(buttonSoundClips[index]);
    }

    public bool GetUsingKeys()
    {
        return usingKeys;
    }
    #endregion
}