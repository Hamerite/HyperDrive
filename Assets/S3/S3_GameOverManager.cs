using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S3_GameOverManager : MonoBehaviour
{
    public AudioClip saveSound;
    public InputField nameInput;

    Button[] buttons;
    Text scoreText;
    Text newHighScore;
    Text highScoreText;
    GameManager gameManager;
    AudioSource audioSource;

    readonly WaitForSeconds timer = new WaitForSeconds(0.15f);

    string champName;

    void Awake()
    {
        buttons = FindObjectsOfType<Button>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();

        audioSource.clip = saveSound;
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<Text>();
        newHighScore = GameObject.FindGameObjectWithTag("NewHighScore").GetComponent<Text>();
        newHighScore.gameObject.SetActive(false);
    }

    void Start()
    {
        if (PlayerPrefs.GetString("ChampName") == null)
            champName = "";
        else
            champName = PlayerPrefs.GetString("ChampName");

        if (PlayerPrefs.GetInt("HighScore") < gameManager.GetNumbers("Score"))
        {
            PlayerPrefs.SetInt("HighScore", gameManager.GetNumbers("Score"));
            newHighScore.gameObject.SetActive(true);

            StartCoroutine(FlashingLetters());
            ButtonsInteractability();
        }

        scoreText.text = "Score: " + gameManager.GetNumbers("Score").ToString();
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
        gameManager.TraverseScenes(3, 2);
    }

    public void MainMenuButton()
    {
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(3, 1);
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