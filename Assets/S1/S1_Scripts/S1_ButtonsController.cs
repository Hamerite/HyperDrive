//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class S1_ButtonsController : MonoBehaviour
{
    public static S1_ButtonsController Instance { get; private set; }

    [SerializeField] Text titleText;
    [SerializeField] GameObject[] startMenus;

    EventSystem eventSystem;

    bool panelChange;

    void Awake()
    {
        Instance = this;
        eventSystem = EventSystem.current;
    }

    void Start()
    {
        startMenus[0].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            BackButton();
    }

    public void ChangePanels(string text, bool s0, bool s1, bool s2, bool s3, bool s4)
    {
        panelChange = true;
        AudioManager.Instance.PlayButtonSound(1);

        titleText.text = text;
        startMenus[0].SetActive(s0);
        startMenus[1].SetActive(s1);
        startMenus[2].SetActive(s2);
        startMenus[3].SetActive(s3);
        startMenus[4].SetActive(s4);

        if (Cursor.visible)
            eventSystem.SetSelectedGameObject(null);
        else
        {
            if (MenusManager.Instance.GetSelectedButton())
                MenusManager.Instance.GetSelectedButton().Select();
            else
                MenusManager.Instance.GetSelectedToggle().Select();
        }
        panelChange = false;
    }

    public void SetPanelChange()
    {
        panelChange = true;
    }

    public void ButtonSelected()
    {
        if(!panelChange)
            AudioManager.Instance.PlayButtonSound(0);
    }

    public void BackButton()
    {
        ChangePanels("HyperDrive", true, false, false, false, false);

        panelChange = true;
    }
}