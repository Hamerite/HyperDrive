//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S3_ButtonsController : MonoBehaviour {
    public static S3_ButtonsController Instance { get; private set; }

    [SerializeField] protected Button[] buttons;
    [SerializeField] protected InputField nameInput;

    void Awake() { Instance = this; }

    public void Start() { MenusManager.Instance.SetSelectedButton(buttons[0], null, nameInput.isFocused); }

    public void LoadNewScene(string name) {
        AudioManager.Instance.PlayInteractionSound(1);
        GameManager.Instance.TraverseScenes(name);
    }

    public void ButtonsInteractability() { for (int i = 0; i < buttons.Length; i++) { buttons[i].interactable = !buttons[i].interactable; } }

    public void ButtonSelected() {
        if (!buttons[0].interactable) return;
        AudioManager.Instance.PlayInteractionSound(0);
    }

    public void HighScoreAcheived() {
        nameInput.Select();
        nameInput.ActivateInputField();
        MenusManager.Instance.InputingHighscore = true;
    }

    public void SetName() {
        AudioManager.Instance.PlayInteractionSound(2);
        S3_GameOverManager.Instance.SetChampName(nameInput.text);

        ButtonsInteractability();
        MenusManager.Instance.InputingHighscore = false;

        nameInput.enabled = false;
        ButtonsInteractability();
        Start();
    }
}
