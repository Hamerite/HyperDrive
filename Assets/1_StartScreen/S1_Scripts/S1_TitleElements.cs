//Created by Dylan LeClair
//Last modified 11-08-21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_TitleElements : MonoBehaviour {
    [SerializeField] protected Button selectedButton = null;

    void OnEnable() { Invoke(nameof(CheckForButtonSelected), 0.001f); }

    void CheckForButtonSelected() { MenusManager.Instance.SetSelectedButton(selectedButton, null, false); }

    public void StartButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        GameManager.Instance.TraverseScenes("MainScene");
    }

    public void InstructionsButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        S1_ButtonsController.Instance.ChangePanels("Instructions", new bool[] { false, true, false, false, true }); 
    }

    public void ShipSelect() {
        AudioManager.Instance.PlayInteractionSound(1);
        S1_ButtonsController.Instance.ChangePanels("Ship Selection", new bool[] { false, false, true, false, true }); 
    }

    public void SoundTrackButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        GameManager.Instance.TraverseScenes("Music");
    }

    public void OptionsButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        S1_ButtonsController.Instance.ChangePanels("Ship Selection", new bool[] { false, false, false, true, true }); 
    }

    public void QuitGame(){
        AudioManager.Instance.PlayInteractionSound(1);
        Application.Quit();
    }
}
