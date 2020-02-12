using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S1_ButtonsController : MonoBehaviour
{
    GameManager gameManager;

    Text titleText;
    
    GameObject titleButtons;
    GameObject controls;
    GameObject selectScreen;
    readonly List<GameObject> ships = new List<GameObject>();

    int index;
    Vector3 center;

    Vector3 rotaion = new Vector3(0, 70, 0);

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        titleText = GameObject.FindGameObjectWithTag("TitleText").GetComponent<Text>();
        titleButtons = GameObject.FindGameObjectWithTag("TitleButtons");
        controls = GameObject.FindGameObjectWithTag("Controls");
        selectScreen = GameObject.FindGameObjectWithTag("SelectScreen");

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

    private void Update()
    {
        center = new Vector3(Camera.main.pixelWidth / 100, Camera.main.pixelHeight / 100, 700);

        foreach (GameObject item in ships)
        {
            item.transform.Rotate(rotaion * Time.deltaTime);
        }
    }

    public void StartButton()
    {
        gameManager.ResetGame("Reset");
        gameManager.TraverseScenes(0, 1);
    }

    public void ControlsButton()
    {
        titleText.text = "Controls";
        titleButtons.SetActive(false);
        controls.SetActive(true);
    }

    public void ShipSelect()
    {
        titleText.text = "Ship Selection";
        selectScreen.SetActive(true);
        titleButtons.SetActive(false);

        ships[PlayerPrefs.GetInt("Selection")].SetActive(true);
        ships[PlayerPrefs.GetInt("Selection")].transform.position = center;

        index = PlayerPrefs.GetInt("Selection");
    }

    public void SelectButton()
    {
        titleText.text = "HyperDrive";
        ships[index].SetActive(false);
        selectScreen.SetActive(false);
        titleButtons.SetActive(true);

        PlayerPrefs.SetInt("Selection", index);
        PlayerPrefs.Save();
    }

    public void Previous()
    {
        ships[index].SetActive(false);
        index--;

        if(index < 0)
            index = ships.Count - 1;

        ships[index].SetActive(true);
        ships[index].transform.position = center;
    }

    public void Next()
    {
        ships[index].SetActive(false);
        index++;

        if(index > ships.Count - 1)
            index = 0;

        ships[index].SetActive(true);
        ships[index].transform.position = center;
    }

    public void BackButton()
    {
        titleText.text = "HyperDrive";
        titleButtons.SetActive(true);
        controls.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}