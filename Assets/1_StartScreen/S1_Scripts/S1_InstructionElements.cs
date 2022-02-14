//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_InstructionElements : MonoBehaviour {
    [SerializeField] protected GameObject[] InstructionElements;
    [SerializeField] protected Button[] buttons;
    [SerializeField] protected CanvasGroup[] canvasgroups;

    void OnEnable() { MenusManager.Instance.SetSelectedButton(buttons[1], null, false); }

    public void ToggleInstructionsTab(){
        AudioManager.Instance.PlayInteractionSound(1);

        for (int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = !buttons[i].interactable;
            canvasgroups[i].interactable = !canvasgroups[i].interactable;
            canvasgroups[i].blocksRaycasts = !canvasgroups[i].blocksRaycasts;
            InstructionElements[i].SetActive(!InstructionElements[i].activeSelf);

            if (buttons[i].interactable) MenusManager.Instance.SetSelectedButton(buttons[i], null, false);
        }

        S1_ButtonsController.Instance.ChangePanels(new bool[] { false, true, false, true });
    }
}
