//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour {
    public static MenusManager Instance { get; private set; }

    protected Vector3 mousePos;
    protected Button newSelectedButton;
    protected Toggle newSelectedToggle;

    protected bool inputingHighscore;

    void Start() {
        Instance = this;
        mousePos = Input.mousePosition;
    }

    void Update() {
        if (inputingHighscore || SceneManager.GetActiveScene().name == "MainScene") return;

        if (Cursor.visible && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) MouseToKeys();
        else if (!Cursor.visible && mousePos != Input.mousePosition) KeysToMouse();
    }

    void MouseToKeys() {
        mousePos = Input.mousePosition;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (newSelectedButton) newSelectedButton.Select();
        else if (newSelectedToggle) newSelectedToggle.Select();
    }

    void KeysToMouse() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetSelectedButton(Button selectedButton, Toggle selectedToggle) {
        newSelectedButton = selectedButton;
        newSelectedToggle = selectedToggle;

        if (!selectedButton & !selectedToggle) EventSystem.current.SetSelectedGameObject(null);
    }

    public Button GetSelectedButton() { return newSelectedButton; }

    public Toggle GetSelectedToggle() { return newSelectedToggle; }

    public void ToggleInputingHighscore() { inputingHighscore = !inputingHighscore; }
}
