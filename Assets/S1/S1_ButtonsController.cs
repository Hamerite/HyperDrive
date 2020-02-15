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

    Text titleText;
    Toggle firstToggle;

    readonly List<GameObject> InstructionElements = new List<GameObject>();
    readonly List<GameObject> ships = new List<GameObject>();
    readonly List<GameObject> startMenus =  new List<GameObject>();

    Vector3 center;
    Vector3 mousePos;
    Vector3 rotaion = new Vector3(0, 70, 0);

    bool usingKeys;
    int index;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        titleText = GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>();

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

        MouseToKeys();
        KeysToMouse();
    }

    public void BackButton()
    {
        gameManager.ButtonPressed();
        if(Cursor.visible == false)
        {
            firstButton = startMenus[0].GetComponentsInChildren<Button>();
            firstButton[0].Select();
        }

        titleText.text = "HyperDrive";
        startMenus[0].SetActive(true);
        for (int i = 1; i < startMenus.Count - 1; i++)
        {
            startMenus[i].SetActive(false);
        }
        ships[index].SetActive(false);
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
        if (Cursor.visible == false)
            InstructionElements[1].GetComponent<Button>().Select();

        titleText.text = "Instructions";
        startMenus[1].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);

        InstructionElements[3].SetActive(false);
    }

    public void ShipSelect()
    {
        gameManager.ButtonPressed();
        if(Cursor.visible == false)
        {
            firstButton = startMenus[2].GetComponentsInChildren<Button>();
            firstButton[0].Select();
        }

        titleText.text = "Ship Selection";
        startMenus[2].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);

        ships[PlayerPrefs.GetInt("Selection")].SetActive(true);
        ships[PlayerPrefs.GetInt("Selection")].transform.position = center;
        index = PlayerPrefs.GetInt("Selection");
    }

    public void SoundTrackButton()
    {
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(1, 4);
    }

    public void OptionsButton()
    {
        gameManager.ButtonPressed();
        if(Cursor.visible == false)
        {
            firstToggle = startMenus[3].GetComponentInChildren<Toggle>();
            firstToggle.Select();
        }

        titleText.text = "Options";
        startMenus[3].SetActive(true);
        startMenus[4].SetActive(true);
        startMenus[0].SetActive(false);
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

        titleText.text = "HyperDrive";
        ships[index].SetActive(false);
        startMenus[0].SetActive(true);
        startMenus[2].SetActive(false);
        startMenus[4].SetActive(false);

        PlayerPrefs.SetInt("Selection", index);
        PlayerPrefs.Save();
    }

    public void Previous()
    {
        gameManager.ButtonPressed();

        ships[index].SetActive(false);
        index--;

        if (index < 0)
            index = ships.Count - 1;

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

        ships[index].SetActive(true);
        ships[index].transform.position = center;
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

    #region Helper Fuctions
    public void ButtonSelected()
    {
        audioSource.clip = s1Clips[0];
        audioSource.Play();
    }

    public void ResetHighScores()
    {
        audioSource.clip = s1Clips[1];
        audioSource.Play();

        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("ChampName");
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
        if(mousePos != Input.mousePosition)
        {
            Cursor.visible = true;
            usingKeys = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    #endregion
}