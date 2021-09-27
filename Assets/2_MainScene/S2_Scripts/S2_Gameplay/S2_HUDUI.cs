//Created by Dylan LeClair 31/05/21
//Last modified 31/05/21 (Dylan LeClair)
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S2_HUDUI : MonoBehaviour {
    public static S2_HUDUI Instance { get; private set; }

    [SerializeField] protected Text[] textElements = null; //{ Score, Obstacles, Level, Thruster }
    [SerializeField] protected Slider[] shipAttributes = null; //{ Shields, Health, Thrusters }
    
    protected int score, passedCounter;
    protected float countDown;

    void Awake() { Instance = this; }

    void Start() { Invoke(nameof(TurnOnHUD), 3); }

    void TurnOnHUD() {
        for (int i = 0; i < textElements.Length - 1; i++) textElements[i].gameObject.SetActive(true);
        foreach (Slider item in shipAttributes) item.gameObject.SetActive(true);

        for (int i = 0; i < shipAttributes.Length - 1; i++) shipAttributes[i].value = ShipStats.Instance.GetStats().GetAttributes()[i + 1];
    }

    public void SetScore(int add) { 
        score += add;
        textElements[0].text = "Score: " +  score.ToString();
    }

    public void SetObstacleCounter() { 
        passedCounter++;
        textElements[1].text = "Obstacles Passed: " + passedCounter.ToString();
    }

    public int GetObstacleCounter() { return passedCounter; }

    public void SetLevel(string level) { textElements[2].text = "Level: " + level; }

    public void SendGameInfo() { GameManager.Instance.SetScoreVariables(score, passedCounter); }

    public void SetAttributes(int index, int valueChange) { shipAttributes[index].value += valueChange; }

    public Slider[] GetAttributes() { return shipAttributes; }

    public void StartCountdownTimer() {
        textElements[3].gameObject.SetActive(true);
        countDown = ShipStats.Instance.GetStats().GetAttributes()[4];
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() {
        do {
            countDown -= Time.deltaTime;
            textElements[3].text = countDown.ToString("F2");
            yield return null;
        } while (countDown > 0);

        textElements[3].gameObject.SetActive(false);
    }
}
