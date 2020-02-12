using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S3_GameOverManager : MonoBehaviour
{
    GameManager gameManager;
    S2_PoolController poolController;

    Text scoreText;
    Text highScoreText;
    Text newHighScore;

    InputField nameInput;
    string champName;

    readonly WaitForSeconds timer = new WaitForSeconds(0.15f);

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        poolController = FindObjectOfType<S2_PoolController>();

        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<Text>();

        newHighScore = GameObject.FindGameObjectWithTag("NewHighScore").GetComponent<Text>();
        newHighScore.gameObject.SetActive(false);

        nameInput = GameObject.FindGameObjectWithTag("NameInput").GetComponent<InputField>();
    }

    void Start()
    {
        if (PlayerPrefs.GetString("ChampName") == null)
            champName = "";
        else
            champName = PlayerPrefs.GetString("ChampName");

        if (PlayerPrefs.GetInt("HighScore") < gameManager.GetScore())
        {
            PlayerPrefs.SetInt("HighScore", gameManager.GetScore());
            newHighScore.gameObject.SetActive(true);
            StartCoroutine(FlashingLetters());
        }

        scoreText.text = "Score: " + gameManager.GetScore().ToString();
        highScoreText.text = "High Score: " + champName + PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void PlayAgainButton()
    {
        gameManager.ResetGame("Reset");
        gameManager.TraverseScenes(2, 1);

        poolController.ResetArrays();
    }

    public void MainMenuButton()
    {
        gameManager.TraverseScenes(2, 0);
    }

    public void SetName()
    {
        champName = nameInput.text;
    }

    IEnumerator FlashingLetters()
    {
        yield return timer;
        newHighScore.color = Color.red;
        yield return timer;
        newHighScore.color = Color.black;

        StartCoroutine(FlashingLetters());
    }
}