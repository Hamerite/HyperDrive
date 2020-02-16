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

    AudioSource audioSource;
    GameManager instance;
    S1_ButtonsController S1_ButtonsController;
    Music_ButtonController music_ButtonController;
    S3_GameOverManager S3_GameOverManager;

    Text scoreText;

    bool s2Start;
    int score;
    int counter;
    int coins;

    void Awake()
    {
        if (instance != null && instance != this)
            DestroyImmediate(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        mute.isOn = (PlayerPrefs.GetInt("Mute") != 0);
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));

        coins = PlayerPrefs.GetInt("Coins");
    }

    void Update()
    {
        if(s2Start && SceneManager.GetActiveScene().buildIndex == 2)
        {
            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
            s2Start = false;
        }
        if (scoreText)
            scoreText.text = "Score: " + score.ToString();
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
    #endregion

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

    #region Helper Fuctions
    public void TraverseScenes(int unload, int load)
    {
        SceneManager.LoadScene(load);
        SceneManager.UnloadSceneAsync(unload);

        if(Cursor.visible == false)
        {
            if (load == 1)
            {
                S1_ButtonsController = FindObjectOfType<S1_ButtonsController>();
                List<GameObject> startElements = S1_ButtonsController.GetStartMenus();
                startElements[0].GetComponent<Button>().Select();
            }
            if (load == 3)
            {
                S3_GameOverManager = FindObjectOfType<S3_GameOverManager>();
                Button overButton = S3_GameOverManager.GetPlayButton();
                overButton.Select();
            }
            if(load == 4)
            {
                music_ButtonController = FindObjectOfType<Music_ButtonController>();
                Button musicMain = music_ButtonController.GetFirstPlayButton();
                musicMain.Select();
            }
        }

        if (load == 2)
        {
            s2Start = true;
            score = 0;
            counter = 0;
        }
    }

    public void ButtonPressed()
    {
        audioSource.clip = Pressed;
        audioSource.Play();
    }
    #endregion
}