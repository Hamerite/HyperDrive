//Created by Dylan LeClair 31/05/21
//Last modified 29/09/21 (Dylan LeClair)
//Modified 10/20/21 (Kyle Ennis)
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S2_HUDUI : MonoBehaviour {
    public static S2_HUDUI Instance { get; protected set; }

    [SerializeField] protected Text[] textElements; //{ Score, Obstacles, Level, Thruster }
    [SerializeField] protected Slider[] shipAttributes; //{ Shields, Health, Thrusters }

    protected bool rapidKillRutineRunning;
    protected float countDown, timer;
    protected string diffultyLevelName;

    public int[] RunInfo { get; protected set; } // { Score, PassedCounter, EnemiesKilled, DifficultyPassed, HighestKillCount, RapidKillCount }
    public float GetAttributesValue(int x) { return shipAttributes[x].value; }
    public Slider[] GetAttributes() { return shipAttributes; }

    void Awake() {
        Instance = this;
        RunInfo = new int[6];
    }

    void Start() { Invoke(nameof(TurnOnHUD), 3); }

    void TurnOnHUD() {
        for (int i = 0; i < textElements.Length - 1; i++) textElements[i].gameObject.SetActive(true);
        for (int i = 0; i < shipAttributes.Length; i++) { shipAttributes[i].gameObject.SetActive(true); }
        for (int i = 0; i < shipAttributes.Length - 1; i++) shipAttributes[i].value = ShipStats.Instance.GetStats().GetAttributes()[i + 1];
    }

    public void SendGameInfo() { 
        GameManager.Instance.SetScoreVariables(RunInfo);
        PlayerInfoManager.Instance.SetInfoValues(RunInfo, diffultyLevelName);
    }

    public void StartCountdownTimer() {
        textElements[3].gameObject.SetActive(true);
        countDown = ShipStats.Instance.GetStats().GetAttributes()[4];
        StartCoroutine(Countdown());
    }

    public void EnemyKilled(int value) {
        RunInfo[2]++;

        if (RunInfo[5] >= 5) { value *= 2; }
        RunInfo[0] += value;
        textElements[0].text = "Score: " + RunInfo[0].ToString();

        S2_EnemyManager.Instance.SetEnemiesLive();
        if(S2_EnemyManager.Instance.GetEnemiesLive() <= 0) { S2_EnemyManager.Instance.EndWave(); }

        RunInfo[5]++;
        timer = 3;
        if (!rapidKillRutineRunning) StartCoroutine(KillsMultiplierTimer());
    }

    public void SetScore(int value) {
        RunInfo[0] += value;
        textElements[0].text = "Score: " + RunInfo[0].ToString();
    }

    public void SetObstacleCounter() {
        RunInfo[1]++;
        textElements[1].text = "Obstacles Passed: " + RunInfo[1].ToString();
    }

    public void SetLevel(string level) { 
        textElements[2].text = "Level: " + level;
        diffultyLevelName = level;
        RunInfo[3]++;

        if(RunInfo[3] > 1) RunInfo[0] += (int)(shipAttributes[0].value + shipAttributes[1].value);
    }

    public void SetAttributes(int index, int valueChange) { shipAttributes[index].value += valueChange; }

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

        if (RunInfo[5] > RunInfo[4]) RunInfo[4] = RunInfo[5];
        RunInfo[5] = 0;
        rapidKillRutineRunning = false;
    }
}
