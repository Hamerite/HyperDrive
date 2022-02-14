//Created by Dylan LeClair
//Last modified 11-08-21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_TitleElements : MonoBehaviour {
    [SerializeField] protected Button selectedButton;

    void OnEnable() { Invoke(nameof(CheckForButtonSelected), 0.001f); }

    void CheckForButtonSelected() { MenusManager.Instance.SetSelectedButton(selectedButton, null, false); }

    public void LoadNewScene(string name) {
        AudioManager.Instance.PlayInteractionSound(1);
        GameManager.Instance.TraverseScenes(name);
    }

    public void SwitchNewPanel(int index) {
        AudioManager.Instance.PlayInteractionSound(1);
        if(index == 0) { S1_ButtonsController.Instance.ChangePanels(new bool[] { false, true, false, true }); } //Instructions
        else { S1_ButtonsController.Instance.ChangePanels(new bool[] { false, false, true, true }); } //Options
    }

    public void QuitGame(){
        AudioManager.Instance.PlayInteractionSound(1);
        Application.Quit();
    }
}
