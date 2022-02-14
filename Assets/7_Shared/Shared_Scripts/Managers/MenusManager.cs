//Created by Dylan LeClair
//Last modified 11-08-21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour {
    public static MenusManager Instance { get; private set; }

    protected Vector3 mousePos;
    protected Button newSelectedButton;
    protected Toggle newSelectedToggle;

    public bool InputingHighscore { protected get; set; }
    public bool UsingMouse { get; set; }

    void Awake() {
        Instance = this;
        mousePos = Input.mousePosition;
    }

    void Update() {
        if (InputingHighscore || SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "LoadingScene") return;

        if (Cursor.visible && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) ToggleNavigation(false);
        else if (!Cursor.visible && mousePos != Input.mousePosition) ToggleNavigation(true);
    }

    void ToggleNavigation(bool KBM) {
        Cursor.visible = KBM;

        if (KBM) {
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else {
            mousePos = Input.mousePosition;
            Cursor.lockState = CursorLockMode.Locked;

            if (newSelectedButton) EventSystem.current.SetSelectedGameObject(newSelectedButton.gameObject);
            else if (newSelectedToggle) EventSystem.current.SetSelectedGameObject(newSelectedToggle.gameObject);
        }
    }

    public void SetSelectedButton(Button selectedButton, Toggle selectedToggle, bool inputingHighscore) {
        newSelectedButton = selectedButton;
        newSelectedToggle = selectedToggle;

        if (!Cursor.visible && !inputingHighscore) {
            if (selectedButton) EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
            else if (selectedToggle) EventSystem.current.SetSelectedGameObject(selectedToggle.gameObject);
        }
        else EventSystem.current.SetSelectedGameObject(null);
    }
}
