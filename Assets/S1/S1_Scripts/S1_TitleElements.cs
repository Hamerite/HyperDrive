//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class S1_TitleElements : MonoBehaviour
{
    [SerializeField] Button selectedButton;

    void OnEnable()
    {
        if (!Cursor.visible)
            selectedButton.Select();
        MenusManager.Instance.SetSelectedButton(selectedButton, null);
    }

    public void StartButton()
    {
        AudioManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(1, 2);
    }

    public void InstructionsButton()
    {
        S1_ButtonsController.Instance.ChangePanels("Instructions", false, true, false, false, true);
    }

    public void ShipSelect()
    {
        S1_ButtonsController.Instance.ChangePanels("Ship Selection", false, false, true, false, true);
    }

    public void SoundTrackButton()
    {
        AudioManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(1, 4);
    }

    public void OptionsButton()
    {
        S1_ButtonsController.Instance.ChangePanels("Ship Selection", false, false, false, true, true);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayButtonSound(1);

        Application.Quit();
    }
}
