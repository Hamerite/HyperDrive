//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance { get; private set; }

    Vector3 mousePos;
    Button newSelectedButton;
    Toggle newSelectedToggle;

    void Start()
    {
        Instance = this;
        mousePos = Input.mousePosition;
    }

    void Update()
    {
        if (Cursor.visible && Input.GetAxis("Vertical") != 0 || Cursor.visible && Input.GetAxis("Horizontal") != 0)
            MouseToKeys();
        else if (!Cursor.visible && mousePos != Input.mousePosition)
            KeysToMouse();
    }

    void MouseToKeys()
    {
        mousePos = Input.mousePosition;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (newSelectedButton)
            newSelectedButton.Select();
        else if (newSelectedToggle)
            newSelectedToggle.Select();
    }
    void KeysToMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetSelectedButton(Button selectedButton, Toggle selectedToggle)
    {
        newSelectedButton = selectedButton;
        newSelectedToggle = selectedToggle;
    }
    public Button GetSelectedButton()
    {
        return newSelectedButton;
    }
    public Toggle GetSelectedToggle()
    {
        return newSelectedToggle;
    }
}
