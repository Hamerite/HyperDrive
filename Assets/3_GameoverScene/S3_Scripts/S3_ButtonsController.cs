﻿//Created by Dylan LeClair
//Last modified 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S3_ButtonsController : MonoBehaviour {
    public static S3_ButtonsController Instance { get; private set; }

    [SerializeField] protected Button[] buttons = null;
    [SerializeField] protected InputField nameInput = null;

    void Awake() { Instance = this; }

    void Start() { if (!Cursor.visible && !nameInput.isFocused) buttons[0].Select(); }

    public void PlayAgainButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        GameManager.Instance.TraverseScenes("MainScene");
    }

    public void MainMenuButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        GameManager.Instance.TraverseScenes("StartScreen");
    }

    public void ButtonsInteractability() { foreach (Button item in buttons) item.interactable = !item.interactable; }

    public void ButtonSelected() {
        if (!buttons[0].interactable) return;
        AudioManager.Instance.PlayInteractionSound(0); 
    }

    public void HighScoreAcheived() {
        nameInput.Select();
        nameInput.ActivateInputField();
        MenusManager.Instance.ToggleInputingHighscore();
    }

    public void SetName() {
        AudioManager.Instance.PlayInteractionSound(2);

        nameInput.enabled = false;
        S3_GameOverManager.Instance.SetChampName(nameInput.text);

        ButtonsInteractability();
        MenusManager.Instance.ToggleInputingHighscore();

        Start();
    }
}
