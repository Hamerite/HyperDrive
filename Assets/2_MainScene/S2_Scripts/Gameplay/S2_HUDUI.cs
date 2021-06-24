//Created by Dylan LeClair 31/05/21
//Last modified 31/05/21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S2_HUDUI : MonoBehaviour {
    public static S2_HUDUI Instance { get; private set; }

    [SerializeField] protected Text scoreText, obstaclesPassed, levelReached;
    
    protected int score, passedCounter;

    void Awake() { Instance = this; }

    public void SetScore(int add) { 
        score += add;
        scoreText.text = "Score: " +  score.ToString();
    }

    public void SetObstacleCounter() { 
        passedCounter++;
        obstaclesPassed.text = "Obstacles Passed: " + passedCounter.ToString();
    }

    public int GetObstacleCounter() { return passedCounter; }

    public void SetLevel(string level) { levelReached.text = "Level: " + level; }

    public void SendGameInfo() { GameManager.Instance.SetScoreVariables(score, passedCounter); }
}
