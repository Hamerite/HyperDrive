//Created by Dylan LeClair
//Last revised 11-08-21 (Dylan LeClair)
using UnityEngine;

public class S1_ButtonsController : MonoBehaviour {
    public static S1_ButtonsController Instance { get; private set; }

    [SerializeField] protected GameObject[] startMenus;

    protected bool panelChange;

    void Awake() { Instance = this; }

    void Update() { if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) BackButton(); }

    public void ChangePanels(bool[] status) {
        SetPanelChange();
        AudioManager.Instance.PlayInteractionSound(1);

        for (int i = 0; i < startMenus.Length; i++) { startMenus[i].SetActive(status[i]); }
    }

    public void SetPanelChange() {
        if (!panelChange) {
            panelChange = true;
            Invoke(nameof(SetPanelChange), 0.01f);
        }
        else { panelChange = false; }
    }

    public void ButtonSelected() {
        if (panelChange) return;
        AudioManager.Instance.PlayInteractionSound(0); 
    }

    public void BackButton(){
        AudioManager.Instance.PlayInteractionSound(7);

        if(S1_Options.Instance && S1_Options.Instance.GetResetCheck()) return;

        if(startMenus[3].activeInHierarchy) AudioManager.Instance.SaveAudioSettings();
        ChangePanels(new bool[] { true, false, false, false });
    }
}