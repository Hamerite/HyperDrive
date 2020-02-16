using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class S1_ButtonsController : MonoBehaviour
{
    [SerializeField]
    AudioClip[] s1Clips;

    AudioSource audioSource;
    GameManager gameManager;

    Button[] firstButton;
    Slider[] volumeSliders;

    Text titleText;
    Text price;
    Text coinCount;
    Toggle firstToggle;
    GameObject resetCheck;

    readonly List<GameObject> InstructionElements = new List<GameObject>();
    readonly List<GameObject> ships = new List<GameObject>();
    readonly List<GameObject> startMenus =  new List<GameObject>();

    Vector3 center;
    Vector3 mousePos;
    Vector3 rotaion = new Vector3(0, 70, 0);
    
    readonly bool[] wasPurchased = { true, false, false, false };
    public int[] prices = { 0, 10000, 10000, 10000 };

    bool afford;
    bool usingKeys;
    int index;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        titleText = GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>();
        resetCheck = GameObject.FindGameObjectWithTag("ResetCheck");
        coinCount = GameObject.FindGameObjectWithTag("Score1").GetComponent<Text>();
        price = GameObject.FindGameObjectWithTag("Price").GetComponent<Text>();

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("ShipSelect"))
        {
            ships.Add(item);
            item.SetActive(false);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Instructions"))
        {
            InstructionElements.Add(item);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("StartMenus"))
        {
            startMenus.Add(item);
            item.SetActive(false);
        }
    }

    void Start()
    {
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        InstructionElements[0].GetComponent<Button>().interactable = false;
        startMenus[0].SetActive(true);

        wasPurchased[1] = (PlayerPrefs.GetInt("SharkPurchased") != 0);
        wasPurchased[2] = (PlayerPrefs.GetInt("BattlePurchased") != 0);
        wasPurchased[3] = (PlayerPrefs.GetInt("XPurchased") != 0);
    }

    void Update()
    {
        center = new Vector3(Camera.main.pixelWidth / 100, Camera.main.pixelHeight / 100, 700);

        foreach (GameObject item in ships)
        {
            item.transform.Rotate(rotaion * Time.deltaTime);
        }

        if (startMenus[0].activeInHierarchy)
            startMenus[4].SetActive(false);
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            BackButton();

        MouseToKeys();
        KeysToMouse();
    }

    public void BackButton()
    {
        gameManager.ButtonPressed();

        titleText.text = "HyperDrive";
        startMenus[0].SetActive(true);
        ships[index].SetActive(false);
        resetCheck.SetActive(false);
        for (int i = 1; i < startMenus.Count - 1; i++)
        {
            startMenus[i].SetActive(false);
        }

        if(Cursor.visible == false)
        {
            firstButton = startMenus[0].GetComponentsInChildren<Button>();
            firstButton[0].Select();
        }
    }

    #region Title Elements
    public void StartButton()
    {
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(1, 2);
    }

    public void InstructionsButton()
    {
        gameManager.ButtonPressed();

        titleText.text = "Instructions";
        startMenus[1].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);
        InstructionElements[3].SetActive(false);

        if (Cursor.visible == false)
            InstructionElements[1].GetComponent<Button>().Select();
    }

    public void ShipSelect()
    {
        gameManager.ButtonPressed();

        titleText.text = "Ship Selection";
        startMenus[2].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);

        firstButton = startMenus[2].GetComponentsInChildren<Button>();
        if(Cursor.visible == false)
        {
            firstButton[0].Select();
        }

        ships[PlayerPrefs.GetInt("Selection")].SetActive(true);
        ships[PlayerPrefs.GetInt("Selection")].transform.position = center;
        index = PlayerPrefs.GetInt("Selection");

        if(index == 0)
            price.text = "";

        coinCount.text = "Coins: " + gameManager.GetNumbers("Coins");
    }

    public void SoundTrackButton()
    {
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(1, 4);
    }

    public void OptionsButton()
    {
        gameManager.ButtonPressed();

        titleText.text = "Options";
        startMenus[3].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);
        resetCheck.SetActive(false);

        volumeSliders = startMenus[3].GetComponentsInChildren<Slider>();
        volumeSliders[0].value = Mathf.Pow(10, PlayerPrefs.GetFloat("MasterVolume")) * 2;
        volumeSliders[1].value = Mathf.Pow(10, PlayerPrefs.GetFloat("MusicVolume")) * 2;
        volumeSliders[2].value = Mathf.Pow(10, PlayerPrefs.GetFloat("SFXVolume")) * 2;

        if (Cursor.visible == false)
        {
            firstToggle = startMenus[3].GetComponentInChildren<Toggle>();
            firstToggle.Select();
        }
    }

    public void QuitGame()
    {
        gameManager.ButtonPressed();

        Application.Quit();
    }
    #endregion

    #region Ship Selection
    public void SelectButton()
    {
        gameManager.ButtonPressed();

        if(!afford)
            CheckPurchse();
        else
        {
            titleText.text = "HyperDrive";
            ships[index].SetActive(false);
            startMenus[0].SetActive(true);
            startMenus[2].SetActive(false);
            startMenus[4].SetActive(false);

            PlayerPrefs.SetInt("Selection", index);
            PlayerPrefs.Save();

            afford = false;
        }
    }

    public void Previous()
    {
        gameManager.ButtonPressed();

        ships[index].SetActive(false);
        index--;

        if (index < 0)
            index = ships.Count - 1;

        if (wasPurchased[index] == false)
        {
            firstButton[0].GetComponentInChildren<Text>().text = "BUY";
            price.text = "Costs " + prices[index] + " Coins";
        }
        else
        {
            firstButton[0].GetComponentInChildren<Text>().text = "SELECT";
            price.text = "";
        }

        ships[index].SetActive(true);
        ships[index].transform.position = center;
    }

    public void Next()
    {
        gameManager.ButtonPressed();

        ships[index].SetActive(false);
        index++;

        if (index > ships.Count - 1)
            index = 0;

        if (wasPurchased[index] == false)
        {
            firstButton[0].GetComponentInChildren<Text>().text = "BUY";
            price.text = "Costs " + prices[index] + " Coins";
        }
        else
        {
            firstButton[0].GetComponentInChildren<Text>().text = "SELECT";
            price.text = "";
        }

        ships[index].SetActive(true);
        ships[index].transform.position = center;
    }

    void CheckPurchse()
    {

        if (wasPurchased[index] == true)
        {
            afford = true;
            SelectButton();
        }
        else if (wasPurchased[index] == false && gameManager.GetNumbers("Coins") > prices[index])
        {
            wasPurchased[index] = true;

            afford = true;
            SavePurchase();
        }
        else
        {

        }
    }

    void SavePurchase()
    {
        if(index == 1)
            PlayerPrefs.SetInt("SharkPurchased", (wasPurchased[index] ? 1 : 0));
        if(index == 2)
            PlayerPrefs.SetInt("BattlePurchased", (wasPurchased[index] ? 1 : 0));
        if(index == 3)
            PlayerPrefs.SetInt("XPurchased", (wasPurchased[index] ? 1 : 0));

        PlayerPrefs.Save();
        SelectButton();
    }
    #endregion

    #region Instruction Elements
    public void ControlsButton()
    {
        InstructionElements[1].GetComponent<Button>().interactable = true;
        InstructionElements[0].GetComponent<Button>().interactable = false;

        InstructionElements[2].SetActive(true);
        InstructionElements[3].SetActive(false);
    }

    public void ScoringButton()
    {
        InstructionElements[0].GetComponent<Button>().interactable = true;
        InstructionElements[1].GetComponent<Button>().interactable = false;

        InstructionElements[3].SetActive(true);
        InstructionElements[2].SetActive(false);
    }
    #endregion

    #region Delete Highscore Functions
    public void ResetHighScores()
    {
        resetCheck.SetActive(true);

        if(Cursor.visible == false)
        {
            Button[] selectedButton = resetCheck.GetComponentsInChildren<Button>();
            selectedButton[0].Select();
        }
    }

    public void DeleteButton()
    {
        audioSource.clip = s1Clips[1];
        audioSource.Play();

        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("ChampName");

        resetCheck.SetActive(false);
    }

    public void CancelButton()
    {
        resetCheck.SetActive(false);
    }
    #endregion

    #region Helper Fuctions
    public void ButtonSelected()
    {
        audioSource.clip = s1Clips[0];
        audioSource.Play();
    }

    public List<GameObject> GetStartMenus()
    {
        return startMenus;
    }

    void MouseToKeys()
    {
        if (!usingKeys && Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if(!usingKeys)
                mousePos = Input.mousePosition;

            usingKeys = true;
            Cursor.visible = false;

            if (startMenus[0].activeInHierarchy)
            {
                firstButton = startMenus[0].GetComponentsInChildren<Button>();
                firstButton[0].Select();
            }
            if (startMenus[1].activeInHierarchy)
            {
                if (InstructionElements[0].GetComponent<Button>().interactable)
                    InstructionElements[0].GetComponent<Button>().Select();
                else
                    InstructionElements[1].GetComponent<Button>().Select();
            }
            if (startMenus[2].activeInHierarchy)
            {
                firstButton = startMenus[2].GetComponentsInChildren<Button>();
                firstButton[0].Select();
            }
            if (startMenus[3].activeInHierarchy)
            {
                firstToggle = startMenus[3].GetComponentInChildren<Toggle>();
                firstToggle.Select();
            }
        }
    }

    void KeysToMouse()
    {
        if(usingKeys && mousePos != Input.mousePosition)
        {
            Cursor.visible = true;
            usingKeys = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    #endregion
}