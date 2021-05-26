//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    protected Text scoreText;
    protected Text obstaclesPassed;
    protected Text levelReached;

    protected bool s2Start;
    protected int score;
    protected int counter;
    protected int coins;
    protected string levelText;

    void Awake() {
        if (Instance != null && Instance != this) DestroyImmediate(gameObject);
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SGD data = GDSM.LoadData();
        if (data != null) coins = data.coins;
    }

    void Update() {
        if(s2Start && SceneManager.GetActiveScene().buildIndex == 2) {
            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
            obstaclesPassed = GameObject.FindGameObjectWithTag("ObstaclesPassed").GetComponent<Text>();
            levelReached = GameObject.FindGameObjectWithTag("LevelReached").GetComponent<Text>();
            s2Start = false;
        }
        if (scoreText) scoreText.text = "Score: " + score.ToString();
        if (obstaclesPassed) obstaclesPassed.text = "Obstacle: " + counter.ToString();
        if (levelReached) levelReached.text = "Level: " + levelText;
    }

    public int GetNumbers(string operation) {
        if (operation == "Score") return score;
        if (operation == "Counter") return counter;
        if (operation == "Coins") return coins;
        return 0;
    }

    public void SetNumbers(string operation, int add) {
        if (operation == "Score") score += add;
        if (operation == "Counter") counter++;
        if (operation == "Coins") {
            coins += (score * 5) + (counter * 2);
            GDSM.SaveData(this);
        }
    }

    public void SetLevel(string level) { levelText = level; }

    public void TraverseScenes(int load) {
        SceneManager.LoadScene(load, LoadSceneMode.Single);

        if (load == 2) {
            s2Start = true;
            score = 0;
            counter = 0;
        }
    }
}