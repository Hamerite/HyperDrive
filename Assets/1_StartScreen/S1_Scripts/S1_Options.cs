//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_Options : MonoBehaviour {
    [SerializeField] protected GameObject resetCheck = null;
    [SerializeField] protected Button selectedButton = null;
    [SerializeField] protected Toggle[] mutes = null; // { All, Menu }
    [SerializeField] protected Slider[] volumeSliders = null; // { Master, Music, SFX }

    void Start() { resetCheck.SetActive(false); }

    void OnEnable() {
        if(!Cursor.visible) mutes[0].Select();
        MenusManager.Instance.SetSelectedButton(null, mutes[0]);

        for (int i = 0; i < 2; i++) mutes[i].isOn = AudioManager.Instance.GetMutes()[i];
        for (int i = 0; i < 3; i++) volumeSliders[i].value = AudioManager.Instance.GetVolumes()[i];
    }

    void Update() {
        if (!resetCheck.activeInHierarchy) return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
            resetCheck.SetActive(false);
            S1_ButtonsController.Instance.SetPanelChange();
            if (!Cursor.visible) mutes[0].Select();
        }
    }

    public void ResetHighScores() {
        AudioManager.Instance.PlayInteractionSound(1);
        resetCheck.SetActive(true);
        S1_ButtonsController.Instance.SetPanelChange();

        MenusManager.Instance.SetSelectedButton(selectedButton, null);
        if (!Cursor.visible) selectedButton.Select();
    }

    public void DeleteButton() {
        AudioManager.Instance.PlayInteractionSound(3);
        FeedbackMessageController.Instance.SetMessage("SAVE DELETED", Color.red);

        SDSM.DeleteData();

        resetCheck.SetActive(false);
        if (!Cursor.visible) mutes[0].Select();
    }

    public void CancelButton() {
        AudioManager.Instance.PlayInteractionSound(1);

        resetCheck.SetActive(false);
        if (!Cursor.visible) mutes[0].Select();
    }

    #region On Value Change
    public void Mute(bool value) { 
        AudioManager.Instance.SetMute(value);
        if(Cursor.visible) MenusManager.Instance.SetSelectedButton(null, null);
    }
    
    public void MuteMenu(bool value) { 
        AudioManager.Instance.SetMenuMute(value);
        if (Cursor.visible) MenusManager.Instance.SetSelectedButton(null, null);
    }

    public void SetMaster(float value) { 
        AudioManager.Instance.SetMasterVolume(value);
        if (Cursor.visible) MenusManager.Instance.SetSelectedButton(null, null);
    }

    public void SetMusic(float value) { 
        AudioManager.Instance.SetMusicVolume(value);
        if (Cursor.visible) MenusManager.Instance.SetSelectedButton(null, null);
    }

    public void SetSFX(float value) { 
        AudioManager.Instance.SetSFXVolume(value);
        if (Cursor.visible) MenusManager.Instance.SetSelectedButton(null, null);
    }
    #endregion
}
