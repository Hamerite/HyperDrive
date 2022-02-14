//Created by Dylan LeClair
//Last revised 11-08-21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S1_ButtonsController : MonoBehaviour {
    public static S1_ButtonsController Instance { get; private set; }

    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected GameObject[] startMenus;

    protected bool panelChange;

    void Awake() { Instance = this; }

    void Start() { startMenus[0].SetActive(true); }

    void Update() { if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) BackButton(); }

    public void ChangePanels(string text, bool[] status) {
        panelChange = true;
        AudioManager.Instance.PlayInteractionSound(1);

        titleText.text = text;
        for (int i = 0; i < startMenus.Length; i++) { startMenus[i].SetActive(status[i]); }

        panelChange = false;
    }

    public void SetPanelChange() { panelChange = true; }

    public void ButtonSelected() {
        if (panelChange) return;
        AudioManager.Instance.PlayInteractionSound(0); 
    }

    public void BackButton(){
        AudioManager.Instance.PlayInteractionSound(7);

        if(S1_Options.Instance && S1_Options.Instance.GetResetCheck()) return;

        if(startMenus[3].activeInHierarchy) AudioManager.Instance.SaveAudioSettings();
        ChangePanels("HyperDrive", new bool[] { true, false, false, false });
        panelChange = true;
    }
}