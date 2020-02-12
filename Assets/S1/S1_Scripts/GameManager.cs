using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameManager instance;

    Text scoreText;
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

    void Update()
    {
        if(s2Start && SceneManager.GetActiveScene().buildIndex == 1)
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

    public int GetScore()
    {
        return score;
    }
    public void SetScore(int add)
    {
        score += add;
    }

    public bool GetisAlive()
    {
        return isAlive;
    }

    public int GetCounter()
    {
        return counter;
    }
    public void SetCounter()
    {
        counter++;
    }

    public void ResetGame(string operation)
    {
        if(operation == "Reset")
        {
            s2Start = !s2Start;
            score = 0;
        }
        isAlive = !isAlive;
    }
}