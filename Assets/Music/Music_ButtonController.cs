//Created by Dylan LeClair
//Last revised 23-02-20 (Dylan LeClair)

using System.Collections;
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

    readonly WaitForSeconds timer = new WaitForSeconds(0.35f);

    bool canScroll = true;
    int index;
    float verticalPos;
    #endregion

    Button mainMenuButton;
    Button[] songPlayButtons;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        scrollRect = FindObjectOfType<ScrollRect>();

        mainMenuButton = GameObject.FindGameObjectWithTag("Main").GetComponent<Button>();
        songPlayButtons = scrollRect.GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        if (gameManager.GetUsingKeys())
        {
            songPlayButtons[index].Select();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            gameManager.TraverseScenes(4, 1);

        if(Cursor.visible == false)
        {
            if(Input.GetAxis("Vertical") > 0.0f ^ Input.GetAxis("Vertical") < 0.0f)
            {
                if(canScroll)
                {
                    if (Input.GetAxis("Vertical") > 0.0f)
                        index = Mathf.Clamp(index - 1, 0, songPlayButtons.Length - 1);
                    else
                        index = Mathf.Clamp(index + 1, 0, songPlayButtons.Length);

                    if (index == 9)
                        mainMenuButton.Select();
                    else
                        songPlayButtons[index].Select();

                    verticalPos = 1.0f - ((float)index / (songPlayButtons.Length - 1));

                    canScroll = false;
                    StartCoroutine(DelayScroll());
                }
            }
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, verticalPos, Time.deltaTime * 10.0f);
        }

        if (index == 9)
            gameManager.MouseToKeys(mainMenuButton, null);
        else
            gameManager.MouseToKeys(songPlayButtons[index], null);
    }

    IEnumerator DelayScroll()
    {
        yield return timer;

        canScroll = true;
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

    public void SelectedWithKeys()
    {
        if (gameManager.GetUsingKeys())
            gameManager.PlayButtonSound(0);
    }

    public void MainMenuButton()
    {
        gameManager.PlayButtonSound(1);
        gameManager.TraverseScenes(4, 1);
    }
    #endregion
}
