//Created by Dylan LeClair
//Last revised 19-02-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Music_ButtonController : MonoBehaviour
{
    #region Script Cache Variables
    GameManager gameManager;
    MusicPlayer musicPlayer;
    #endregion

    #region Scroll Veiw Variables
    ScrollRect scrollRect;

    int index;
    float verticalPos;
    #endregion

    Button[] songPlayButtons;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        scrollRect = FindObjectOfType<ScrollRect>();
        songPlayButtons = scrollRect.GetComponentsInChildren<Button>();

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

        gameManager.MouseToKeys(songPlayButtons[0], null);
    }


    #region Song Choice
    public void PlayActionable()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(0);
    }

    public void PlayBirthOfAHero()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(1);
    }

    public void PlayDubStep()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(2);
    }

    public void PlayEndlessMotion()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(3);
    }

    public void PlayEvolution()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(4);
    }

    public void PlayExtremeAction()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(5);
    }

    public void PlayHighOctane()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(6);
    }

    public void PlayNewDawn()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(7);
    }

    public void PlaySlowMotion()
    {
        gameManager.PlayButtonSound(1);
        musicPlayer.PlaySong(8);
    }

    #endregion

    #region Button Fuctions
    public void ButtonSelected()
    {
        gameManager.PlayButtonSound(0);
    }

    public void MainMenuButton()
    {
        gameManager.PlayButtonSound(1);
        gameManager.TraverseScenes(4, 1);
    }
    #endregion
}
