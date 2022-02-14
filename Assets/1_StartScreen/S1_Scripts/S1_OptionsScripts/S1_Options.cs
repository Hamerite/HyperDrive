//Created by Dylan LeClair
//Last modified 11-08-21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S1_Options : MonoBehaviour {
    public static S1_Options Instance { get; private set; }

    [SerializeField] protected GameObject settings;
    [SerializeField] protected GameObject stats;
    [SerializeField] protected GameObject resetCheck;

    [SerializeField] protected Button selectedButton; //Cancel button
    [SerializeField] protected Button[] buttonsToToggle;
    [SerializeField] protected Toggle[] mutes; // { All, Menu }
    [SerializeField] protected Slider[] volumeSliders; // { Master, Music, SFX }

    [SerializeField] protected TextMeshProUGUI statsToggleText;

    public bool GetResetCheck { get { return resetCheck.activeInHierarchy; } }

    void Awake() { Instance = this; }

    void OnEnable() {
        for (int i = 0; i < 2; i++) mutes[i].isOn = AudioManager.Instance.Mutes[i];
        for (int i = 0; i < 3; i++) volumeSliders[i].value = AudioManager.Instance.Volumes[i];

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
            DeactivateResetCheck(false);
            S1_ButtonsController.Instance.SetPanelChange();
            MenusManager.Instance.SetSelectedButton(buttonsToToggle[0], null, false);
        }
    }

    public void ToggleStats() {
        AudioManager.Instance.PlayInteractionSound(1);
        if (mutes[0].gameObject.activeInHierarchy) { statsToggleText.text = "SETTINGS"; }
        else { statsToggleText.text = "PLAYER STATS"; }

        settings.SetActive(!settings.activeSelf);
        stats.SetActive(!stats.activeSelf);

        MenusManager.Instance.SetSelectedButton(buttonsToToggle[1], null, false);
    }

    public void ResetHighScores() {
        AudioManager.Instance.PlayInteractionSound(1);
        S1_ButtonsController.Instance.SetPanelChange();

        ToggleInteractables(false);
        resetCheck.SetActive(true);
        MenusManager.Instance.SetSelectedButton(selectedButton, null, false);
    }

    public void DeactivateResetCheck(bool deleted) {
        if (deleted) { //Delete button
            AudioManager.Instance.PlayInteractionSound(3);
            FeedbackMessageController.Instance.SetMessage("SAVE DELETED", Color.red);
            SDSM.DeleteData();
        }
        else { AudioManager.Instance.PlayInteractionSound(1); } //Cancel Button

        ToggleInteractables(true);
        resetCheck.SetActive(false);
        MenusManager.Instance.SetSelectedButton(buttonsToToggle[0], null, false);
    }

    void ToggleInteractables(bool status) {
        for (int i = 0; i < mutes.Length; i++) { mutes[i].interactable = status; }
        for (int i = 0; i < buttonsToToggle.Length; i++){
            buttonsToToggle[i].interactable = status;
            volumeSliders[i].interactable = status;
        }
    }

    #region On Value Change
    public void Mute(bool status) { 
        AudioManager.Instance.SetMutes(null, status, 0);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }
    
    public void MuteMenu(bool status) { 
        AudioManager.Instance.SetMutes("MixerMenu", status, 1);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }

    public void SetMaster(float value) { 
        AudioManager.Instance.SetVolume("MixerMaster", value, 0);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }

    public void SetMusic(float value) {
        AudioManager.Instance.SetVolume("MixerMaster", value, 1);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }

    public void SetSFX(float value) {
        AudioManager.Instance.SetVolume("MixerMaster", value, 2);
        MenusManager.Instance.SetSelectedButton(null, null, false);
    }
    #endregion
}
