//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_InstructionElements : MonoBehaviour {
    [SerializeField] protected GameObject[] InstructionElements = null;
    [SerializeField] protected Button[] buttons = null;
    [SerializeField] protected CanvasGroup[] canvasgroups = null;

    void OnEnable() {
        InstructionElements[3].SetActive(false);

        buttons[0].interactable = false;
        canvasgroups[0].interactable = false;
        canvasgroups[0].blocksRaycasts = false;

        MenusManager.Instance.SetSelectedButton(buttons[1], null);
        if (!Cursor.visible) buttons[1].Select();
    }

    public void ToggleInstructionsTab(){
        AudioManager.Instance.PlayInteractionSound(1);

        for (int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = !buttons[i].interactable;
            canvasgroups[i].interactable = !canvasgroups[i].interactable;
            canvasgroups[i].blocksRaycasts = !canvasgroups[i].blocksRaycasts;
        }
        for (int i = 2; i < InstructionElements.Length; i++) InstructionElements[i].SetActive(!InstructionElements[i].activeSelf);

        S1_ButtonsController.Instance.ChangePanels("Instructions", new bool[] { false, true, false, false, true });

        if (buttons[0].interactable) MenusManager.Instance.SetSelectedButton(buttons[0], null);
        else MenusManager.Instance.SetSelectedButton(buttons[1], null);
    }
}
