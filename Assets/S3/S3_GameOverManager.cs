//Created by Dylan LeClair
//Last revised 19-02-20 (Dylan LeClair)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S3_GameOverManager : MonoBehaviour
{
    GameManager gameManager;
    Button[] buttons;

    #region Audio Variabels
    AudioSource audioSource;

    [SerializeField]
    AudioClip coinGainSound;
    #endregion

    #region HighScore Variables
    [SerializeField]
    InputField nameInput;

    readonly WaitForSeconds flashingLettersTimer = new WaitForSeconds(0.15f);
    readonly List<GameObject> textElements = new List<GameObject>();

    string champName;
    #endregion

    #region Coins Variables
    Text coinsGain;

    readonly WaitForSeconds addCoinsTimer = new WaitForSeconds(0.7f);

    bool startAdd;
    float preAddedCoins;
    float addedCoins;
    float gameTime;
    float addRate = 100.0f;
    #endregion

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        buttons = FindObjectsOfType<Button>();
        coinsGain = GameObject.FindGameObjectWithTag("Score1").GetComponent<Text>();

        audioSource.clip = coinGainSound;
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("ScoreText"))
        {
            textElements.Add(item);
        }
        textElements[1].SetActive(false);
    }

    void Start()
    {
        if (PlayerPrefs.GetString("ChampName") == null)
            champName = "";
        else
            champName = PlayerPrefs.GetString("ChampName");

        textElements[0].GetComponent<Text>().text = "Score: " + gameManager.GetNumbers("Score").ToString();
        textElements[2].GetComponent<Text>().text = "High Score: " + champName + " : " + PlayerPrefs.GetInt("HighScore").ToString();

        if (PlayerPrefs.GetInt("HighScore") < gameManager.GetNumbers("Score"))
        {
            PlayerPrefs.SetInt("HighScore", gameManager.GetNumbers("Score"));

            textElements[1].SetActive(true);
            StartCoroutine(FlashingLetters());
            ButtonsInteractability();

            nameInput.Select();
            nameInput.ActivateInputField();
        }

        preAddedCoins = gameManager.GetNumbers("Coins");
        coinsGain.text = "Coins: " + preAddedCoins;
        gameManager.SetNumbers("Coins", 0);
        addedCoins = gameManager.GetNumbers("Coins") - preAddedCoins;

        gameTime = Time.time;

        if (gameManager.GetUsingKeys())
            buttons[0].Select();
    }

    private void Update()
    {
        if(!nameInput.isFocused)
        {
            gameManager.MouseToKeys(buttons[0], null);
        }

        StartCoroutine(AddCoins());

        if (startAdd)
        {
            audioSource.Play();

            if (Time.time > gameTime + 1.0f)
            {
                gameTime = Time.time;
                addRate += 25.0f;
            }

            if (preAddedCoins < gameManager.GetNumbers("Coins"))
                preAddedCoins += Mathf.Floor(addRate * Time.deltaTime);      

            if (addedCoins > 0)
            {
                addedCoins -= Mathf.Floor(addRate * Time.deltaTime);
                coinsGain.text = "Coins: " + preAddedCoins + " + " + addedCoins;
            }
            else
            {
                coinsGain.text = "Coins: " + preAddedCoins;

                audioSource.Stop();

                startAdd = false;
            }
        }
    }

    IEnumerator AddCoins()
    {
        if (addedCoins > 0)
            coinsGain.text = "Coins: " + preAddedCoins + " + " + addedCoins;
        else
            coinsGain.text = "Coins: " + preAddedCoins;

        yield return addCoinsTimer;
        startAdd = true;
    }

    #region Button Fuctions
    public void PlayAgainButton()
    {
        gameManager.PlayButtonSound(1);
        gameManager.TraverseScenes(3, 2);
    }

    public void MainMenuButton()
    {
        gameManager.PlayButtonSound(1);
        gameManager.TraverseScenes(3, 1);
    }
    void ButtonsInteractability()
    {
        foreach (Button item in buttons)
            item.interactable = !item.interactable;
    }

    public void ButtonSelected()
    {
        if(buttons[0].interactable)
        {
            gameManager.PlayButtonSound(0);
        }
    }
    public void SelectedWithKeys()
    {
        if (gameManager.GetUsingKeys())
            gameManager.PlayButtonSound(0);
    }

    #endregion

    #region HighScore Fuctions
    public void SetName()
    {
        gameManager.PlayButtonSound(2);

        champName = nameInput.text;
        PlayerPrefs.SetString("ChampName", champName);
        PlayerPrefs.Save();

        nameInput.enabled = false;
        textElements[1].SetActive(false);
        textElements[2].GetComponent<Text>().text = "High Score: " + champName + "  " + PlayerPrefs.GetInt("HighScore").ToString();

        ButtonsInteractability();
    }

    IEnumerator FlashingLetters()
    {
        yield return flashingLettersTimer;
        textElements[1].GetComponent<Text>().color = Color.red;
        yield return flashingLettersTimer;
        textElements[1].GetComponent<Text>().color = Color.white;

        StartCoroutine(FlashingLetters());
    }
    #endregion
}