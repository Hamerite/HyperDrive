//Created by Dylan LeClair 31/05/21
//Last modified 31/05/21 (Dylan LeClair)
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S2_HUDUI : MonoBehaviour {
    public static S2_HUDUI Instance { get; private set; }

    [SerializeField] protected Text[] textElements = null; //{ Score, Obstacles, Level, Thruster }
    [SerializeField] protected Slider[] shipAttributes = null; //{ Shields, Health, Thrusters }

    protected bool rapidKillRutineRunning;
    protected int score, passedCounter, difficultyPassed, enemiesKilled, rapidKillCount;
    protected float countDown, timer;

    void Awake() { Instance = this; }

    void Start() { Invoke(nameof(TurnOnHUD), 3); }

    void TurnOnHUD() {
        for (int i = 0; i < textElements.Length - 1; i++) textElements[i].gameObject.SetActive(true);
        foreach (Slider item in shipAttributes) item.gameObject.SetActive(true);

        for (int i = 0; i < shipAttributes.Length - 1; i++) shipAttributes[i].value = ShipStats.Instance.GetStats().GetAttributes()[i + 1];
    }

    public void SendGameInfo() { GameManager.Instance.SetScoreVariables(score, difficultyPassed, passedCounter, enemiesKilled); }

    public void StartCountdownTimer() {
        textElements[3].gameObject.SetActive(true);
        countDown = ShipStats.Instance.GetStats().GetAttributes()[4];
        StartCoroutine(Countdown());
    }

    public void EnemyKilled(int value) {
        enemiesKilled++;

        if (rapidKillCount >= 5) value *= 2;
        score += value;
        textElements[0].text = "Score: " + score.ToString();

        rapidKillCount++;
        timer = 3;
        if (!rapidKillRutineRunning) StartCoroutine(KillsMultiplierTimer());
    }

    public void SetScore(int value) {
        score += value;
        textElements[0].text = "Score: " +  score.ToString();
    }

    public void SetObstacleCounter() { 
        passedCounter++;
        textElements[1].text = "Obstacles Passed: " + passedCounter.ToString();
    }

    public void SetLevel(string level) { 
        textElements[2].text = "Level: " + level;
        difficultyPassed++;

        if(difficultyPassed > 1) score += (int)(shipAttributes[0].value + shipAttributes[1].value);
    }

    public void SetAttributes(int index, int valueChange) { shipAttributes[index].value += valueChange; }

    public int GetObstacleCounter() { return passedCounter; }

    public Slider[] GetAttributes() { return shipAttributes; }

    IEnumerator Countdown() {
        do {
            countDown -= Time.deltaTime;
            textElements[3].text = countDown.ToString("F2");
            yield return null;
        } while (countDown > 0);

        textElements[3].gameObject.SetActive(false);
    }
    
    IEnumerator KillsMultiplierTimer() {
        rapidKillRutineRunning = true;

        do {
            timer -= Time.deltaTime;
            yield return null;
        } while (timer > 0);

        rapidKillCount = 0;
        rapidKillRutineRunning = false;
    }
}
