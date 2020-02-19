//Created by Dylan LeClair
//Last revised 19-02-20 (Dylan LeClair)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S1_ButtonsController : MonoBehaviour
{
    GameManager gameManager;
    Slider[] volumeSliders;

    #region Panel Elements Variables
    Text titleText;
    GameObject resetCheck;

    readonly List<GameObject> startMenus =  new List<GameObject>();
    readonly List<GameObject> InstructionElements = new List<GameObject>();
    #endregion

    #region Ship Selection Variables
    Text price;
    Text coinCount;

    readonly List<GameObject> ships = new List<GameObject>();

    Vector3 center;
    Vector3 rotaion = new Vector3(0, 70, 0);

    readonly bool[] wasPurchased = { true, false, false, false };
    readonly int[] prices = { 0, 10000, 10000, 10000 };

    bool afford;
    int index;
    #endregion

    #region Navigation Method Change Variables
    Button[] firstButton;
    Toggle firstToggle;

    bool panelChange;
    bool firstSelectedisToggle;
    #endregion

    void Awake()
    {
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
        else
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
                BackButton();

        PanelCheck();
    }


    #region Title Elements
    public void StartButton()
    {
        gameManager.PlayButtonSound(1);
        gameManager.TraverseScenes(1, 2);
    }

    public void InstructionsButton()
    {
        gameManager.PlayButtonSound(1);
        panelChange = true;

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
        gameManager.PlayButtonSound(1);
        panelChange = true;

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
        gameManager.PlayButtonSound(1);
        gameManager.TraverseScenes(1, 4);
    }

    public void OptionsButton()
    {
        gameManager.PlayButtonSound(1);
        panelChange = true;

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
        gameManager.PlayButtonSound(1);

        Application.Quit();
    }
    #endregion

    #region Ship Selection
    public void SelectButton()
    {
        gameManager.PlayButtonSound(1);

        if (!afford)
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
        gameManager.PlayButtonSound(1);

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
        gameManager.PlayButtonSound(1);

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
        gameManager.PlayButtonSound(1);
        InstructionElements[1].GetComponent<Button>().interactable = true;
        InstructionElements[0].GetComponent<Button>().interactable = false;

        InstructionElements[2].SetActive(true);
        InstructionElements[3].SetActive(false);
    }

    public void ScoringButton()
    {
        gameManager.PlayButtonSound(1);
        InstructionElements[0].GetComponent<Button>().interactable = true;
        InstructionElements[1].GetComponent<Button>().interactable = false;

        InstructionElements[3].SetActive(true);
        InstructionElements[2].SetActive(false);
    }
    #endregion

    #region Delete Highscore Functions
    public void ResetHighScores()
    {
        gameManager.PlayButtonSound(1);
        resetCheck.SetActive(true);

        if(Cursor.visible == false)
        {
            Button[] selectedButton = resetCheck.GetComponentsInChildren<Button>();
            selectedButton[0].Select();
        }
    }

    public void DeleteButton()
    {
        gameManager.PlayButtonSound(3);

        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("ChampName");

        resetCheck.SetActive(false);
    }

    public void CancelButton()
    {
        resetCheck.SetActive(false);
    }
    #endregion

    #region Shared Button Fuctions
    public void ButtonSelected()
    {
        gameManager.PlayButtonSound(0);
    }

    public void BackButton()
    {
        gameManager.PlayButtonSound(1);

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
    #endregion

    #region Panel Check For Navigation Method Change
    void PanelCheck()
    {
        if(panelChange)
        {
            if (startMenus[0].activeInHierarchy)
            {
                firstSelectedisToggle = false;
                firstButton = startMenus[0].GetComponentsInChildren<Button>();
            }
            if (startMenus[1].activeInHierarchy)
            {
                firstSelectedisToggle = false;
                if (InstructionElements[0].GetComponent<Button>().interactable)
                    firstButton[0] =  InstructionElements[0].GetComponent<Button>();
                else
                    firstButton[0] = InstructionElements[1].GetComponent<Button>();
            }
            if (startMenus[2].activeInHierarchy)
            {
                firstSelectedisToggle = false;
                firstButton = startMenus[2].GetComponentsInChildren<Button>();
            }
            if (startMenus[3].activeInHierarchy)
            {
                firstSelectedisToggle = true;
                firstToggle = startMenus[3].GetComponentInChildren<Toggle>();
                firstToggle.Select();
            }

            panelChange = false;
        }

        if(firstSelectedisToggle)
            gameManager.MouseToKeys(null, firstToggle);
        else
            gameManager.MouseToKeys(firstButton[0], null);
    }
    #endregion
}