//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class S3_ButtonsController : MonoBehaviour
{
    public static S3_ButtonsController Instance { get; private set; }

    [SerializeField] Button[] buttons = null;
    [SerializeField] InputField nameInput = null;

    private void Start()
    {
        Instance = this;

        if (!Cursor.visible)
            buttons[0].Select();
    }

    private void Update()
    {
        if (!nameInput.isFocused)
        {
            MenusManager.Instance.SetSelectedButton(buttons[0], null);
        }
    }

    public void PlayAgainButton()
    {
        AudioManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(2);
    }

    public void MainMenuButton()
    {
        AudioManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(1);
    }

    public void ButtonsInteractability()
    {
        foreach (Button item in buttons)
            item.interactable = !item.interactable;
    }

    public void ButtonSelected()
    {
        if (buttons[0].interactable)
            AudioManager.Instance.PlayButtonSound(0);
    }

    public void HighScoreAcheived()
    {
        nameInput.Select();
        nameInput.ActivateInputField();
    }

    public void SetName()
    {
        AudioManager.Instance.PlayButtonSound(2);

        PlayerPrefs.SetString("ChampName", nameInput.text);
        PlayerPrefs.Save();

        nameInput.enabled = false;
        S3_GameOverManager.Instance.SetChampName(nameInput.text);

        ButtonsInteractability();
    }
}
