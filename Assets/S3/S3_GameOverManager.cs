using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class S3_GameOverManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;
    [SerializeField]
    InputField nameInput;

    Button[] buttons;

    Text coinsGain;
    GameManager gameManager;
    AudioSource audioSource;

    readonly WaitForSeconds addCoinsTimer = new WaitForSeconds(0.7f);
    readonly WaitForSeconds flashingLettersTimer = new WaitForSeconds(0.15f);
    readonly List<GameObject> textElements = new List<GameObject>();

    bool startAdd;
    bool usingKeys;
    string champName;
    float preAddedCoins;
    float addedCoins;
    float gameTime;
    float addRate = 100.0f;

    Vector3 mousePos;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        buttons = FindObjectsOfType<Button>();
        coinsGain = GameObject.FindGameObjectWithTag("Score1").GetComponent<Text>();

        audioSource.clip = clips[0];
        audioSource.loop = false;
        audioSource.playOnAwake = false;

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
        textElements[2].GetComponent<Text>().text = "High Score: " + champName + "  " + PlayerPrefs.GetInt("HighScore").ToString();

        if (PlayerPrefs.GetInt("HighScore") < gameManager.GetNumbers("Score"))
        {
            PlayerPrefs.SetInt("HighScore", gameManager.GetNumbers("Score"));

            textElements[1].SetActive(true);
            StartCoroutine(FlashingLetters());
            ButtonsInteractability();
        }

        preAddedCoins = gameManager.GetNumbers("Coins");
        coinsGain.text = "Coins: " + preAddedCoins;
        gameManager.SetNumbers("Coins", 0);
        addedCoins = gameManager.GetNumbers("Coins") - preAddedCoins;

        gameTime = Time.time;
    }

    private void Update()
    {
        if(!nameInput.isFocused)
        {
            MouseToKeys();
            KeysToMouse();
        }

        StartCoroutine(AddCoins());

        if (startAdd)
        {
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

                audioSource.clip = clips[2];
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                coinsGain.text = "Coins: " + preAddedCoins;

                audioSource.Stop();
                audioSource.loop = false;

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
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(3, 2);
    }

    public void MainMenuButton()
    {
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(3, 1);
    }
    void ButtonsInteractability()
    {
        foreach (Button item in buttons)
            item.interactable = !item.interactable;
    }

    public void ButtonSelected()
    {
        if(buttons[0].interactable && buttons[1].interactable)
        {
            audioSource.clip = clips[1];
            audioSource.Play();
        }
    }
    #endregion

    #region HighScore Fuctions
    public void SetName()
    {
        audioSource.clip = clips[0];
        audioSource.Play();

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

    #region Navigation Functions
    void MouseToKeys()
    {
        if (!usingKeys && Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (!usingKeys)
                mousePos = Input.mousePosition;

            usingKeys = true;
            Cursor.visible = false;

            buttons[0].Select();
        }
    }

    void KeysToMouse()
    {
        if (usingKeys && mousePos != Input.mousePosition)
        {
            Cursor.visible = true;
            usingKeys = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public Button GetPlayButton()
    {
        return buttons[0];
    }
    #endregion
}