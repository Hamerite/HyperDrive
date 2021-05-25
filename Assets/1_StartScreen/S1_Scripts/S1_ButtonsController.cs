//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class S1_ButtonsController : MonoBehaviour {
    public static S1_ButtonsController Instance { get; private set; }

    [SerializeField] Text titleText = null;
    [SerializeField] GameObject[] startMenus = null;

    EventSystem eventSystem;
    bool panelChange;

    void Awake() {
        Instance = this;
        eventSystem = EventSystem.current;
    }

    void Start() { startMenus[0].SetActive(true); }

    void Update() { if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) BackButton(); }

    public void ChangePanels(string text, bool[] status) {
        panelChange = true;
        AudioManager.Instance.PlayButtonSound(1);

        titleText.text = text;
        for (int i = 0; i < startMenus.Length; i++) { startMenus[i].SetActive(status[i]); }

        if (Cursor.visible) eventSystem.SetSelectedGameObject(null);
        else{
            if (MenusManager.Instance.GetSelectedButton()) MenusManager.Instance.GetSelectedButton().Select();
            else MenusManager.Instance.GetSelectedToggle().Select();
        }
        panelChange = false;
    }

    public void SetPanelChange() { panelChange = true; }

    public void ButtonSelected() { if(!panelChange) AudioManager.Instance.PlayButtonSound(0); }

    public void BackButton(){
        ChangePanels("HyperDrive", new bool[] { true, false, false, false, false });
        panelChange = true;
    }
}