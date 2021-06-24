//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S3_GameOverManager : MonoBehaviour {
    public static S3_GameOverManager Instance { get; private set; }

    [SerializeField] protected Text[] textElements = null;
    [SerializeField] protected Text coinsGain = null;

    protected int highScore, score, coins;
    protected float preAddedCoins, addedCoins, gameTime, addRate = 300.0f;
    protected string champName;

    void Awake() { 
        Instance = this;

        SSD data = SDSM.LoadData();
        if (data == null) return;

        highScore = data.highScore;
        champName = data.champName;  
    }

    void Start() {
        score = GameManager.Instance.GetScoreAmount();
        coins = GameManager.Instance.GetCoinAmount();

        textElements[0].text = "Score: " + score.ToString();
        textElements[2].text = "High Score: " + champName + " : " + highScore;

        preAddedCoins = coins;
        coinsGain.text = "Coins: " + preAddedCoins;
        GameManager.Instance.CalculateCoins();
        coins = GameManager.Instance.GetCoinAmount();
        addedCoins = coins - preAddedCoins;

        gameTime = Time.time;
        Invoke(nameof(AddCoins), 0.7f);

        if (highScore > score) return;
        highScore = score;

        textElements[1].gameObject.SetActive(true);
        InvokeRepeating(nameof(FlashingLetters), 0.15f, 0.15f);
        S3_ButtonsController.Instance.ButtonsInteractability();
        S3_ButtonsController.Instance.HighScoreAcheived();
    }

    void AddCoins() {
        if (addedCoins == 0) return;

        AudioManager.Instance.PlayLoopingAudio(5);
        StartCoroutine(CoinsCountUp());
    }

    IEnumerator CoinsCountUp() {
        do {
            preAddedCoins += Mathf.Floor(addRate * Time.deltaTime);
            addedCoins -= Mathf.Floor(addRate * Time.deltaTime);
            coinsGain.text = "Coins: " + preAddedCoins + " + " + addedCoins;

            if (Time.time > gameTime + 1.0f) {
                gameTime = Time.time;
                addRate += 25.0f;
            }

            yield return null;
        } while (preAddedCoins < coins);

        coinsGain.text = "Coins: " + preAddedCoins;
        AudioManager.Instance.StopLoopingAudio();
    }

    void FlashingLetters(){
        if(textElements[1].color == Color.red) textElements[1].color = Color.white;
        else textElements[1].color = Color.red;
    }

    public void SetChampName(string name) {
        champName = name;
        textElements[1].gameObject.SetActive(false);
        textElements[2].gameObject.GetComponent<Text>().text = "High Score: " + name + "  " + highScore;
        SaveScoreData();
    }

    void SaveScoreData() { SDSM.SaveData(this); }

    public int GetHighScore() { return highScore; }

    public string GetChampName() { return champName; }
}