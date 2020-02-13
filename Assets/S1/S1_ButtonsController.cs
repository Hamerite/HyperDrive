using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S1_ButtonsController : MonoBehaviour
{
    Text titleText;
    GameObject controls;
    GameObject selectScreen;
    GameObject titleButtons;
    GameManager gameManager;

    readonly List<GameObject> ships = new List<GameObject>();

    Vector3 center;
    Vector3 rotaion = new Vector3(0, 70, 0);

    int index;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        controls = GameObject.FindGameObjectWithTag("Controls");
        titleButtons = GameObject.FindGameObjectWithTag("TitleButtons");
        selectScreen = GameObject.FindGameObjectWithTag("SelectScreen");
        titleText = GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>();

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("ShipSelect"))
        {
            ships.Add(item);
            item.SetActive(false);
        }
    }

    void Start()
    {
        controls.SetActive(false);
        selectScreen.SetActive(false);
    }

    void Update()
    {
        center = new Vector3(Camera.main.pixelWidth / 100, Camera.main.pixelHeight / 100, 700);

        foreach (GameObject item in ships)
        {
            item.transform.Rotate(rotaion * Time.deltaTime);
        }
    }

    public void StartButton()
    {
        gameManager.ButtonPressed();

        gameManager.ResetGame("Reset");
        gameManager.TraverseScenes(1, 2);
    }

    public void ControlsButton()
    {
        gameManager.ButtonPressed();

        titleText.text = "Controls";
        controls.SetActive(true);
        titleButtons.SetActive(false);
    }

    public void ShipSelect()
    {
        gameManager.ButtonPressed();

        titleText.text = "Ship Selection";
        selectScreen.SetActive(true);
        titleButtons.SetActive(false);

        ships[PlayerPrefs.GetInt("Selection")].SetActive(true);
        ships[PlayerPrefs.GetInt("Selection")].transform.position = center;
        index = PlayerPrefs.GetInt("Selection");
    }

    public void SelectButton()
    {
        gameManager.ButtonPressed();

        titleText.text = "HyperDrive";
        ships[index].SetActive(false);
        selectScreen.SetActive(false);
        titleButtons.SetActive(true);

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

    public void BackButton()
    {
        gameManager.ButtonPressed();

        titleText.text = "HyperDrive";
        titleButtons.SetActive(true);
        controls.SetActive(false);
    }

    public void QuitGame()
    {
        gameManager.ButtonPressed();

        Application.Quit();
    }
}