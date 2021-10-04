//Created by Dylan LeClair
//Last modified 11-08-21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_Options : MonoBehaviour {
    public static S1_Options Instance { get; private set; }

    [SerializeField] protected GameObject settings = null;
    [SerializeField] protected GameObject stats = null;
    [SerializeField] protected GameObject resetCheck = null;

    [SerializeField] protected Button selectedButton = null;
    [SerializeField] protected Button[] buttonsToToggle = null;
    [SerializeField] protected Toggle[] mutes = null; // { All, Menu }
    [SerializeField] protected Slider[] volumeSliders = null; // { Master, Music, SFX }

    [SerializeField] protected Text statsToggleText = null;

    void Awake() { Instance = this; }

    void OnEnable() {
        for (int i = 0; i < 2; i++) mutes[i].isOn = AudioManager.Instance.GetMutes()[i];
        for (int i = 0; i < 3; i++) volumeSliders[i].value = AudioManager.Instance.GetVolumes()[i];

        Invoke(nameof(CheckForButtonSelected), 0.001f);
    }

    void OnDisable() {
        statsToggleText.text = "Player Stats";
        settings.SetActive(true);
        stats.SetActive(false);
    }

    void CheckForButtonSelected() { MenusManager.Instance.SetSelectedButton(null, mutes[0], false); }

    void Update() {
        if (!resetCheck.activeInHierarchy) return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
            DeactivateResetCheck();
            S1_ButtonsController.Instance.SetPanelChange();
            MenusManager.Instance.SetSelectedButton(buttonsToToggle[0], null, false);
        }
    }

    public void ToggleStats() {
        AudioManager.Instance.PlayInteractionSound(1);
        if (mutes[0].gameObject.activeInHierarchy) {
            statsToggleText.text = "Settings";
            settings.SetActive(false);
            stats.SetActive(true);
        }
        else {
            statsToggleText.text = "Player Stats";
            settings.SetActive(true);
            stats.SetActive(false);
        }

        MenusManager.Instance.SetSelectedButton(buttonsToToggle[1], null, false);
    }

    public void ResetHighScores() {
        AudioManager.Instance.PlayInteractionSound(1);
        resetCheck.SetActive(true);
        S1_ButtonsController.Instance.SetPanelChange();

        foreach (Button item in buttonsToToggle) item.interactable = false;
        foreach (Toggle item in mutes) item.interactable = false;
        foreach (Slider item in volumeSliders) item.interactable = false;

        MenusManager.Instance.SetSelectedButton(selectedButton, null, false);
    }

    public void DeleteButton() {
        AudioManager.Instance.PlayInteractionSound(3);
        FeedbackMessageController.Instance.SetMessage("SAVE DELETED", Color.red);

        SDSM.DeleteData();
        DeactivateResetCheck();
    }

    public void CancelButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        DeactivateResetCheck();
    }

    void DeactivateResetCheck() {
        foreach (Button item in buttonsToToggle) item.interactable = true;
        foreach (Toggle item in mutes) item.interactable = true;
        foreach (Slider item in volumeSliders) item.interactable = true;

        resetCheck.SetActive(false);
        MenusManager.Instance.SetSelectedButton(buttonsToToggle[0], null, false);
    }

    public bool GetResetCheck() { return resetCheck.activeInHierarchy; }

    #region On Value Change
    public void Mute(bool value) { 
        AudioManager.Instance.SetMute(value);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }
    
    public void MuteMenu(bool value) { 
        AudioManager.Instance.SetMenuMute(value);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }

    public void SetMaster(float value) { 
        AudioManager.Instance.SetMasterVolume(value);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }

    public void SetMusic(float value) { 
        AudioManager.Instance.SetMusicVolume(value);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }

    public void SetSFX(float value) { 
        AudioManager.Instance.SetSFXVolume(value);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }
    #endregion
}
