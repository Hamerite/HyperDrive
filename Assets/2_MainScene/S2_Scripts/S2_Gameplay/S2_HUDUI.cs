//Created by Dylan LeClair 31/05/21
//Last modified 31/05/21 (Dylan LeClair)
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S2_HUDUI : MonoBehaviour {
    public static S2_HUDUI Instance { get; private set; }

    [SerializeField] protected Text scoreText, obstaclesPassed, levelReached, thrusterCountdown;
    [SerializeField] protected Slider[] shipAttributes = null; //{ Shields, Health, Thrusters }
    
    protected int score, passedCounter;
    protected float countDown;

    void Awake() { Instance = this; }

    void Start() { for (int i = 0; i < shipAttributes.Length - 1; i++) shipAttributes[i].value = ShipStats.Instance.GetStats().GetAttributes()[i + 1]; }

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

    public void SetAttributes(int index, int valueChange) { shipAttributes[index].value += valueChange; }

    public Slider[] GetAttributes() { return shipAttributes; }

    public void StartCountdownTimer() {
        thrusterCountdown.gameObject.SetActive(true);
        countDown = ShipStats.Instance.GetStats().GetAttributes()[4];
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() {
        do {
            countDown -= Time.deltaTime;
            thrusterCountdown.text = countDown.ToString("F2");
            yield return null;
        } while (countDown > 0);

        thrusterCountdown.gameObject.SetActive(false);
    }
}
