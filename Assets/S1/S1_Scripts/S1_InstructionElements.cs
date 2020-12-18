//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class S1_InstructionElements : MonoBehaviour
{
    [SerializeField] GameObject[] InstructionElements = null;
    [SerializeField] Button[] buttons = null;
    [SerializeField] CanvasGroup[] canvasgroups = null;

    void OnEnable()
    {
        InstructionElements[3].SetActive(false);

        buttons[0].interactable = false;
        canvasgroups[0].interactable = false;
        canvasgroups[0].blocksRaycasts = false;

        if (!Cursor.visible)
            buttons[1].Select();
        MenusManager.Instance.SetSelectedButton(buttons[1], null);
    }

    public void ToggleInstructionsTab()
    {
        AudioManager.Instance.PlayButtonSound(1);

        buttons[0].interactable = !buttons[0].interactable;
        canvasgroups[0].interactable = !canvasgroups[0].interactable;
        canvasgroups[0].blocksRaycasts = !canvasgroups[0].blocksRaycasts;

        buttons[1].interactable = !buttons[1].interactable;
        canvasgroups[1].interactable = !canvasgroups[1].interactable;
        canvasgroups[1].blocksRaycasts = !canvasgroups[1].blocksRaycasts;

        InstructionElements[2].SetActive(!InstructionElements[2].activeSelf);
        InstructionElements[3].SetActive(!InstructionElements[3].activeSelf);

        S1_ButtonsController.Instance.ChangePanels("Instructions", false, true, false, false, true);

        if (buttons[0].interactable)
            MenusManager.Instance.SetSelectedButton(buttons[0], null);
        else
            MenusManager.Instance.SetSelectedButton(buttons[1], null);
    }
}
