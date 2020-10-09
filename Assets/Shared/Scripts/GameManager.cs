//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } 

    Text scoreText;
    Text obstaclesPassed;
    Text levelReached;
    int score;
    int counter;
    int coins;

    string levelText;

    bool s2Start;

    void Awake()
    {
        if (Instance != null && Instance != this)
            DestroyImmediate(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins");
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
    }

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
}