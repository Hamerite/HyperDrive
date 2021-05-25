//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_Options : MonoBehaviour {
    [SerializeField] GameObject resetCheck = null;
    [SerializeField] Button selectedButton = null;
    [SerializeField] Toggle selectedToggle = null;
    [SerializeField] Toggle menuMuteToggle = null;
    [SerializeField] Slider[] volumeSliders = null;

    void Start() { resetCheck.SetActive(false); }

    void OnEnable() {
        if(!Cursor.visible) selectedToggle.Select();
        MenusManager.Instance.SetSelectedButton(null, selectedToggle);

        selectedToggle.isOn = AudioManager.Instance.GetMutes()[0];
        menuMuteToggle.isOn = AudioManager.Instance.GetMutes()[1];
        volumeSliders[0].value = AudioManager.Instance.GetVolumes()[0];
        volumeSliders[1].value = AudioManager.Instance.GetVolumes()[1];
        volumeSliders[2].value = AudioManager.Instance.GetVolumes()[2];
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && resetCheck.activeInHierarchy || Input.GetKeyDown(KeyCode.JoystickButton1) && resetCheck.activeInHierarchy) {
            resetCheck.SetActive(false);
            if (!Cursor.visible) selectedToggle.Select();
            S1_ButtonsController.Instance.SetPanelChange();
        }
    }

    public void ResetHighScores() {
        AudioManager.Instance.PlayButtonSound(1);
        resetCheck.SetActive(true);
        S1_ButtonsController.Instance.SetPanelChange();

        if (!Cursor.visible) selectedButton.Select();
        MenusManager.Instance.SetSelectedButton(selectedButton, null);
    }

    public void DeleteButton() {
        AudioManager.Instance.PlayButtonSound(3);

        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("ChampName");

        resetCheck.SetActive(false);
        if (!Cursor.visible) selectedToggle.Select();
        MenusManager.Instance.SetSelectedButton(null, selectedToggle);
    }

    public void CancelButton() {
        AudioManager.Instance.PlayButtonSound(1);

        resetCheck.SetActive(false);
        if (!Cursor.visible) selectedToggle.Select();
        MenusManager.Instance.SetSelectedButton(null, selectedToggle);
    }

    #region On Value Change
    public void Mute(bool value) { AudioManager.Instance.SetMute(value); }
    
    public void MuteMenu(bool value) { AudioManager.Instance.SetMenuMute(value); }

    public void SetMaster(float value) { AudioManager.Instance.SetMasterVolume(value); }

    public void SetMusic(float value) { AudioManager.Instance.SetMusicVolume(value); }

    public void SetSFX(float value) { AudioManager.Instance.SetSFXVolume(value); }
    #endregion
}
