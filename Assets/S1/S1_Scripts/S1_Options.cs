//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class S1_Options : MonoBehaviour
{
    [SerializeField] GameObject resetCheck = null;
    [SerializeField] Button selectedButton = null;
    [SerializeField] Toggle selectedToggle = null;
    [SerializeField] Slider[] volumeSliders = null;

    void Start()
    {
        resetCheck.SetActive(false);
    }

    void OnEnable()
    {
        if(!Cursor.visible)
            selectedToggle.Select();
        MenusManager.Instance.SetSelectedButton(null, selectedToggle);

        selectedToggle.isOn = (PlayerPrefs.GetInt("Mute") != 0);
        volumeSliders[0].value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        volumeSliders[1].value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        volumeSliders[2].value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && resetCheck.activeInHierarchy ||
            Input.GetKeyDown(KeyCode.JoystickButton1) && resetCheck.activeInHierarchy)
        {
            resetCheck.SetActive(false);
            if (!Cursor.visible)
                selectedToggle.Select();
            S1_ButtonsController.Instance.SetPanelChange();
        }
    }

    public void ResetHighScores()
    {
        AudioManager.Instance.PlayButtonSound(1);
        resetCheck.SetActive(true);
        S1_ButtonsController.Instance.SetPanelChange();

        if (!Cursor.visible)
            selectedButton.Select();
        MenusManager.Instance.SetSelectedButton(selectedButton, null);
    }

    public void DeleteButton()
    {
        AudioManager.Instance.PlayButtonSound(3);

        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("ChampName");

        resetCheck.SetActive(false);
        if (!Cursor.visible)
            selectedToggle.Select();
        MenusManager.Instance.SetSelectedButton(null, selectedToggle);
    }

    public void CancelButton()
    {
        AudioManager.Instance.PlayButtonSound(1);

        resetCheck.SetActive(false);
        if (!Cursor.visible)
            selectedToggle.Select();
        MenusManager.Instance.SetSelectedButton(null, selectedToggle);
    }
}
