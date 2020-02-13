using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S3_GameOverManager : MonoBehaviour
{
    GameManager gameManager;

    public AudioClip saveSound;
    AudioSource audioSource;
    Button[] buttons;

    Text scoreText;
    Text highScoreText;
    Text newHighScore;

    public InputField nameInput;
    string champName;

    readonly WaitForSeconds timer = new WaitForSeconds(0.15f);

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = saveSound;
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<Text>();

        newHighScore = GameObject.FindGameObjectWithTag("NewHighScore").GetComponent<Text>();
        newHighScore.gameObject.SetActive(false);

        buttons = FindObjectsOfType<Button>();
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

            ButtonsInteractability();
        }

        scoreText.text = "Score: " + gameManager.GetScore().ToString();
        highScoreText.text = "High Score: " + champName + "  " + PlayerPrefs.GetInt("HighScore").ToString();
    }

    void ButtonsInteractability()
    {
        foreach (Button item in buttons)
            item.interactable = !item.interactable;
    }

    public void PlayAgainButton()
    {
        gameManager.ButtonPressed();

        gameManager.ResetGame("Reset");
        gameManager.TraverseScenes(2, 1);
    }

    public void MainMenuButton()
    {
        gameManager.ButtonPressed();

        gameManager.TraverseScenes(2, 0);
    }

    public void SetName()
    {
        audioSource.Play();

        champName = nameInput.text;
        PlayerPrefs.SetString("ChampName", champName);
        PlayerPrefs.Save();

        nameInput.enabled = false;
        highScoreText.text = "High Score: " + champName + "  " + PlayerPrefs.GetInt("HighScore").ToString();

        ButtonsInteractability();
        newHighScore.gameObject.SetActive(false);
    }

    IEnumerator FlashingLetters()
    {
        yield return timer;
        newHighScore.color = Color.red;
        yield return timer;
        newHighScore.color = Color.white;

        StartCoroutine(FlashingLetters());
    }
}