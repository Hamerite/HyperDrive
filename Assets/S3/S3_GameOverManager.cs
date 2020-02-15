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

    GameManager gameManager;
    AudioSource audioSource;

    readonly WaitForSeconds timer = new WaitForSeconds(0.15f);
    readonly List<GameObject> textElements = new List<GameObject>();

    bool usingKeys;
    string champName;

    Vector3 mousePos;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        buttons = FindObjectsOfType<Button>();

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

        if (PlayerPrefs.GetInt("HighScore") < gameManager.GetNumbers("Score"))
        {
            PlayerPrefs.SetInt("HighScore", gameManager.GetNumbers("Score"));

            textElements[1].SetActive(true);
            StartCoroutine(FlashingLetters());
            ButtonsInteractability();
        }

        textElements[0].GetComponent<Text>().text = "Score: " + gameManager.GetNumbers("Score").ToString();
        textElements[2].GetComponent<Text>().text = "High Score: " + champName + "  " + PlayerPrefs.GetInt("HighScore").ToString();
    }

    private void Update()
    {
        MouseToKeys();
        KeysToMouse();
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
        audioSource.clip = clips[1];
        audioSource.Play();
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
        yield return timer;
        textElements[1].GetComponent<Text>().color = Color.red;
        yield return timer;
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

            usingKeys = false;
            Cursor.visible = false;

            buttons[0].Select();
        }
    }

    void KeysToMouse()
    {
        if (mousePos != Input.mousePosition)
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