//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)
using UnityEngine;
using System.Collections;
using TMPro;

public class S3_GameOverManager : MonoBehaviour {
    public static S3_GameOverManager Instance { get; private set; }

    [SerializeField] protected TextMeshProUGUI[] textElements = null; //{ Score, NewHighScore, HighScore }
    [SerializeField] protected TextMeshProUGUI coinsGain = null;

    protected int score, coins;
    protected float preAddedCoins, addedCoins, gameTime, addRate = 300.0f;

    public int HighScore { get; private set; }
    public string ChampName { get; private set; }

    void Awake() { 
        Instance = this;

        SSD data = SDSM.LoadData();
        if (data == null) return;

        HighScore = data.highScore;
        ChampName = data.champName;  
    }

    void Start() {
        score = GameManager.Instance.GetScoreAmount();
        coins = GameManager.Instance.GetCoinAmount();

        textElements[0].text = "Score: " + score.ToString();
        textElements[2].text = "High Score: " + ChampName + " : " + HighScore;

        preAddedCoins = coins;
        coinsGain.text = "Coins: " + preAddedCoins;
        GameManager.Instance.CalculateCoins();
        coins = GameManager.Instance.GetCoinAmount();
        addedCoins = coins - preAddedCoins;

        gameTime = Time.time;
        Invoke(nameof(AddCoins), 0.7f);

        if (HighScore >= score) return;
        HighScore = score;

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
        ChampName = name;
        textElements[1].gameObject.SetActive(false);
        textElements[2].text = "High Score: " + name + "  " + HighScore;
        SDSM.SaveData(this);

        S3_ButtonsController.Instance.Start();
    }
}