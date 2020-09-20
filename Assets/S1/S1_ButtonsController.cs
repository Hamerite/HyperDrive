//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class S1_ButtonsController : MonoBehaviour
{
    EventSystem eventSystem;
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
    int playerCoins;
    #endregion

    #region Navigation Method Change Variables
    Button[] firstButton;
    Toggle firstToggle;

    bool panelChange;
    bool firstSelectedisToggle;
    #endregion

    void Awake()
    {
        titleText = GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>();
        resetCheck = GameObject.FindGameObjectWithTag("ResetCheck");
        coinCount = GameObject.FindGameObjectWithTag("Coins").GetComponent<Text>();
        price = GameObject.FindGameObjectWithTag("Price").GetComponent<Text>();

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("ShipSelect"))
        {
            ships.Add(item);
            item.SetActive(false);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Instructions"))
            InstructionElements.Add(item);
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("StartMenus"))
        {
            startMenus.Add(item);
            item.SetActive(false);
        }

        startMenus.TrimExcess();
        InstructionElements.TrimExcess();
        ships.TrimExcess();

        eventSystem = EventSystem.current;
    }

    void Start()
    {
        InstructionElements[0].GetComponent<Button>().interactable = false;
        InstructionElements[0].GetComponent<CanvasGroup>().interactable = false;
        InstructionElements[0].GetComponent<CanvasGroup>().blocksRaycasts = false;

        startMenus[0].SetActive(true);
        firstButton = startMenus[0].GetComponentsInChildren<Button>();
        if (GameManager.Instance.GetUsingKeys())
            firstButton[0].Select();

        wasPurchased[1] = (PlayerPrefs.GetInt("SharkPurchased") != 0);
        wasPurchased[2] = (PlayerPrefs.GetInt("BattlePurchased") != 0);
        wasPurchased[3] = (PlayerPrefs.GetInt("XPurchased") != 0);

        playerCoins = GameManager.Instance.GetNumbers("Coins");
    }

    void Update()
    {
        center = new Vector3(Camera.main.pixelWidth / 100, Camera.main.pixelHeight / 100, 700);

        foreach (GameObject item in ships)
            item.transform.Rotate(rotaion * Time.deltaTime);

        if (startMenus[0].activeInHierarchy)
            startMenus[4].SetActive(false);

        if (Input.GetKeyDown(KeyCode.Escape) && resetCheck.activeInHierarchy || 
            Input.GetKeyDown(KeyCode.JoystickButton1) && resetCheck.activeInHierarchy)
        {
            resetCheck.SetActive(false);
            panelChange = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            BackButton();

        if(afford)
        {
            coinCount.color = Color.red;
            coinCount.text = "Coins: " + playerCoins + " - " + prices[index];
            Invoke(nameof(Purchused), 0.8f);
        }

        PanelCheck();
    }

    public void ToggleInstructionsTab()
    {
        GameManager.Instance.PlayButtonSound(1);

        InstructionElements[1].GetComponent<Button>().interactable = !InstructionElements[1].GetComponent<Button>().interactable;
        InstructionElements[1].GetComponent<CanvasGroup>().interactable = !InstructionElements[1].GetComponent<CanvasGroup>().interactable;
        InstructionElements[1].GetComponent<CanvasGroup>().blocksRaycasts = !InstructionElements[1].GetComponent<CanvasGroup>().blocksRaycasts;

        InstructionElements[0].GetComponent<Button>().interactable = !InstructionElements[0].GetComponent<Button>().interactable;
        InstructionElements[0].GetComponent<CanvasGroup>().interactable = !InstructionElements[0].GetComponent<CanvasGroup>().interactable;
        InstructionElements[0].GetComponent<CanvasGroup>().blocksRaycasts = !InstructionElements[0].GetComponent<CanvasGroup>().blocksRaycasts;

        InstructionElements[2].SetActive(!InstructionElements[2].activeSelf);
        InstructionElements[3].SetActive(!InstructionElements[3].activeSelf);

        panelChange = true;
    }

    #region Title Elements
    public void StartButton()
    {
        GameManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(1, 2);
    }

    public void InstructionsButton()
    {
        GameManager.Instance.PlayButtonSound(1);
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
        GameManager.Instance.PlayButtonSound(1);
        panelChange = true;

        titleText.text = "Ship Selection";
        startMenus[2].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);

        firstButton = startMenus[2].GetComponentsInChildren<Button>();
        if(Cursor.visible == false)
            firstButton[0].Select();

        ships[PlayerPrefs.GetInt("Selection")].SetActive(true);
        ships[PlayerPrefs.GetInt("Selection")].transform.position = center;
        index = PlayerPrefs.GetInt("Selection");

        if(index == 0)
            price.text = "";

        coinCount.text = "Coins: " + playerCoins;
    }

    public void SoundTrackButton()
    {
        GameManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(1, 4);
    }

    public void OptionsButton()
    {
        GameManager.Instance.PlayButtonSound(1);
        panelChange = true;

        titleText.text = "Options";
        startMenus[3].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);
        resetCheck.SetActive(false);

        volumeSliders = startMenus[3].GetComponentsInChildren<Slider>();
        volumeSliders[0].value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        volumeSliders[1].value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        volumeSliders[2].value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        if (Cursor.visible == false)
        {
            firstToggle = startMenus[3].GetComponentInChildren<Toggle>();
            firstToggle.Select();
        }
    }

    public void QuitGame()
    {
        GameManager.Instance.PlayButtonSound(1);

        Application.Quit();
    }
    #endregion

    #region Ship Selection
    public void SelectButton()
    {
        if (wasPurchased[index] == true)
        {
            GameManager.Instance.PlayButtonSound(1);

            titleText.text = "HyperDrive";
            ships[index].SetActive(false);
            startMenus[0].SetActive(true);
            startMenus[2].SetActive(false);
            startMenus[4].SetActive(false);

            PlayerPrefs.SetInt("Selection", index);
            PlayerPrefs.Save();

            panelChange = true;
        }
        else if (wasPurchased[index] == false && playerCoins >= prices[index])
        {
            firstButton[0].GetComponentInChildren<Text>().text = "Purchsed";
            price.text = "";

            wasPurchased[index] = true;
            afford = true;
            SavePurchase();
        }
        else
        {
            GameManager.Instance.PlayButtonSound(4);

            if (Cursor.visible)
                eventSystem.SetSelectedGameObject(null);
        }
    }

    public void Previous()
    {
        ships[index].SetActive(false);

        index--;
        if (index < 0)
            index = ships.Count - 1;

        GoToNextShip();
    }

    public void Next()
    {
        ships[index].SetActive(false);

        index++;
        if (index > ships.Count - 1)
            index = 0;

        GoToNextShip();
    }
    void GoToNextShip()
    {
        GameManager.Instance.PlayButtonSound(1);

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

        if (Cursor.visible)
            eventSystem.SetSelectedGameObject(null);
    }

    void SavePurchase()
    {
        if(index == 1)
            PlayerPrefs.SetInt("SharkPurchased", (wasPurchased[index] ? 1 : 0));
        if(index == 2)
            PlayerPrefs.SetInt("BattlePurchased", (wasPurchased[index] ? 1 : 0));
        if(index == 3)
            PlayerPrefs.SetInt("XPurchased", (wasPurchased[index] ? 1 : 0));

        playerCoins -= prices[index];
        GameManager.Instance.SetNumbers("Coins", playerCoins);
        PlayerPrefs.SetInt("Coins", playerCoins);
        PlayerPrefs.Save();
    }

    void Purchused()
    {
        coinCount.color = Color.yellow;
        coinCount.text = "Coins: " + playerCoins;
        afford = false;
    }
    #endregion

    #region Delete Highscore Functions
    public void ResetHighScores()
    {
        GameManager.Instance.PlayButtonSound(1);
        resetCheck.SetActive(true);
        panelChange = true;

        if(Cursor.visible == false)
        {
            Button[] selectedButton = resetCheck.GetComponentsInChildren<Button>();
            selectedButton[0].Select();
        }
    }

    public void DeleteButton()
    {
        GameManager.Instance.PlayButtonSound(3);

        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("ChampName");

        resetCheck.SetActive(false);
        panelChange = true;
    }

    public void CancelButton()
    {
        GameManager.Instance.PlayButtonSound(1);

        resetCheck.SetActive(false);
        panelChange = true;
    }
    #endregion

    #region Shared Button Fuctions
    public void ButtonSelected()
    {
        if(!panelChange)
            GameManager.Instance.PlayButtonSound(0);
    }

    public void BackButton()
    {
        GameManager.Instance.PlayButtonSound(1);

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

        panelChange = true;
    }
    #endregion

    #region Navigation Method Change
    void PanelCheck()
    {
        if(panelChange)
        {
            panelChange = false;

            if (Cursor.visible)
                eventSystem.SetSelectedGameObject(null);
            else if (startMenus[3].activeInHierarchy && !resetCheck.activeInHierarchy)
            {
                firstSelectedisToggle = true;
                firstToggle = startMenus[3].GetComponentInChildren<Toggle>();
                firstToggle.Select();
            }
            else
            {
                firstSelectedisToggle = false;

                if (startMenus[0].activeInHierarchy)
                    firstButton = startMenus[0].GetComponentsInChildren<Button>();
                if (startMenus[1].activeInHierarchy)
                {
                    if (InstructionElements[0].GetComponent<Button>().interactable)
                        firstButton[0] = InstructionElements[0].GetComponent<Button>();
                    else
                        firstButton[0] = InstructionElements[1].GetComponent<Button>();
                }
                if (startMenus[2].activeInHierarchy)
                    firstButton = startMenus[2].GetComponentsInChildren<Button>();
                if (startMenus[3].activeInHierarchy && resetCheck.activeInHierarchy)
                {
                    firstSelectedisToggle = false;
                    firstButton = resetCheck.GetComponentsInChildren<Button>();
                }
            }

            if(firstSelectedisToggle)
                GameManager.Instance.MouseToKeys(null, firstToggle);
            else
                GameManager.Instance.MouseToKeys(firstButton[0], null);
        }
    }
    #endregion
}