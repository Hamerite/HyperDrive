//Created by Dylan LeClair 31/05/21
//Last modified 29/09/21 (Dylan LeClair)
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S2_HUDUI : MonoBehaviour {
    public static S2_HUDUI Instance { get; private set; }

    [SerializeField] protected Text[] textElements = null; //{ Score, Obstacles, Level, Thruster }
    [SerializeField] protected Slider[] shipAttributes = null; //{ Shields, Health, Thrusters }

    protected bool rapidKillRutineRunning;
    int[] runInfo = new int[6]; // { Score, PassedCounter, EnemiesKilled, DifficultyPassed, HighestKillCount, RapidKillCount }
    protected float countDown, timer;
    protected string diffultyLevelName;

    void Awake() { Instance = this; }

    void Start() { Invoke(nameof(TurnOnHUD), 3); }

    void TurnOnHUD() {
        for (int i = 0; i < textElements.Length - 1; i++) textElements[i].gameObject.SetActive(true);
        foreach (Slider item in shipAttributes) item.gameObject.SetActive(true);

        for (int i = 0; i < shipAttributes.Length - 1; i++) shipAttributes[i].value = ShipStats.Instance.GetStats().GetAttributes()[i + 1];
    }

    public void SendGameInfo() { 
        GameManager.Instance.SetScoreVariables(runInfo);
        PlayerInfoManager.Instance.SetInfoValues(runInfo, diffultyLevelName);
    }

    public void StartCountdownTimer() {
        textElements[3].gameObject.SetActive(true);
        countDown = ShipStats.Instance.GetStats().GetAttributes()[4];
        StartCoroutine(Countdown());
    }

    public void EnemyKilled(int value) {
        runInfo[2]++;

        if (runInfo[5] >= 5) value *= 2;
        runInfo[0] += value;
        textElements[0].text = "Score: " + runInfo[0].ToString();

        runInfo[5]++;
        timer = 3;
        if (!rapidKillRutineRunning) StartCoroutine(KillsMultiplierTimer());
    }

    public void SetScore(int value) {
        runInfo[0] += value;
        textElements[0].text = "Score: " + runInfo[0].ToString();
    }

    public void SetObstacleCounter() {
        runInfo[1]++;
        textElements[1].text = "Obstacles Passed: " + runInfo[1].ToString();
    }

    public void SetLevel(string level) { 
        textElements[2].text = "Level: " + level;
        diffultyLevelName = level;
        runInfo[3]++;

        if(runInfo[3] > 1) runInfo[0] += (int)(shipAttributes[0].value + shipAttributes[1].value);
    }

    public void SetAttributes(int index, int valueChange) { shipAttributes[index].value += valueChange; }

    public int GetObstacleCounter() { return runInfo[1]; }

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

        if (runInfo[5] > runInfo[4]) runInfo[4] = runInfo[5];
        runInfo[5] = 0;
        rapidKillRutineRunning = false;
    }
}
