using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Music_ButtonController : MonoBehaviour
{
    [SerializeField]
    AudioClip mouseOverSound;

    AudioSource audioSource;
    GameManager gameManager;
    MusicPlayer musicPlayer;
    ScrollRect scrollRect;
    Button[] songPlayButtons;

    Vector3 mousePos;

    bool usingKeys;
    int index;
    float verticalPos;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        scrollRect = FindObjectOfType<ScrollRect>();
        songPlayButtons = scrollRect.GetComponentsInChildren<Button>();

        audioSource.clip = mouseOverSound;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            gameManager.TraverseScenes(4, 1);

        if(Cursor.visible == false)
        {
            index = System.Array.IndexOf(songPlayButtons, EventSystem.current.currentSelectedGameObject);
            verticalPos = 1.0f - ((float)index / (songPlayButtons.Length - 1));
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, verticalPos, Time.deltaTime / 2.0f);
        }

        MouseToKeys();
        KeysToMouse();
    }

    public Button GetFirstPlayButton()
    {
        return songPlayButtons[0];
    }

    #region Song Choice
    public void PlayActionable()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(0);
    }

    public void PlayBirthOfAHero()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(1);
    }

    public void PlayDubStep()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(2);
    }

    public void PlayEndlessMotion()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(3);
    }

    public void PlayEvolution()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(4);
    }

    public void PlayExtremeAction()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(5);
    }

    public void PlayHighOctane()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(6);
    }

    public void PlayNewDawn()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(7);
    }

    public void PlaySlowMotion()
    {
        gameManager.ButtonPressed();
        musicPlayer.PlaySong(8);
    }

    #endregion

    #region Button Fuctions
    public void ButtonSelected()
    {
        audioSource.Play();
    }

    public void MainMenuButton()
    {
        gameManager.ButtonPressed();
        gameManager.TraverseScenes(4, 1);
    }
    #endregion

    #region Navigation Functions
    void MouseToKeys()
    {
        if (!usingKeys && Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (!usingKeys)
                mousePos = Input.mousePosition;

            usingKeys = true;
            Cursor.visible = false;

            songPlayButtons[0].Select();
        }
    }

    void KeysToMouse()
    {
        if (usingKeys && mousePos != Input.mousePosition)
        {
            Cursor.visible = true;
            usingKeys = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    #endregion
}
