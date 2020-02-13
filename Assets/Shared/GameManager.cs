using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioClip[] clips;

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
    }

    public void ButtonSelected()
    {
        audioSource.clip = clips[0];
        audioSource.Play();
    }
    public void ButtonPressed()
    {
        audioSource.clip = clips[1];
        audioSource.Play();
    }
}