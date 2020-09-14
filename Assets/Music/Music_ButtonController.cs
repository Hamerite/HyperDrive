//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class Music_ButtonController : MonoBehaviour
{
    MusicPlayer musicPlayer;
    Button mainMenuButton;
    Button[] songPlayButtons;

    #region Scroll Veiw Variables
    ScrollRect scrollRect;

    bool canScroll = true;
    bool isUsingKeys;
    int index;
    float verticalPos;
    #endregion

    void Awake()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        scrollRect = FindObjectOfType<ScrollRect>();

        mainMenuButton = GameObject.FindGameObjectWithTag("Main").GetComponent<Button>();
        songPlayButtons = scrollRect.GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        if (GameManager.Instance.GetUsingKeys())
        {
            songPlayButtons[index].Select();
        }

        verticalPos = 1.0f;
        isUsingKeys = GameManager.Instance.GetUsingKeys();

        foreach (Button item in songPlayButtons)
        {
            item.onClick.AddListener(() => PlayTrack(System.Array.IndexOf(songPlayButtons, item)));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            GameManager.Instance.TraverseScenes(4, 1);

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

                    if (isUsingKeys != GameManager.Instance.GetUsingKeys())
                    {
                        index = 0;
                        isUsingKeys = GameManager.Instance.GetUsingKeys();
                    }
                    if (index == 9)
                        mainMenuButton.Select();
                    else
                        songPlayButtons[index].Select();

                    verticalPos = 1.0f - ((float)index / (songPlayButtons.Length - 1));

                    canScroll = false;
                    Invoke(nameof(DelayScroll), 0.35f);
                }
            }
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, verticalPos, Time.deltaTime * 20.0f);
        }

        if (index == 9)
            GameManager.Instance.MouseToKeys(mainMenuButton, null);
        else
            GameManager.Instance.MouseToKeys(songPlayButtons[index], null);
    }

    void DelayScroll()
    {
        canScroll = true;
    }

    public void PlayTrack(int index)
    {
        GameManager.Instance.PlayButtonSound(1);
        musicPlayer.PlaySong(index);
    }

    public void ButtonSelected()
    {
        GameManager.Instance.PlayButtonSound(0);
    }

    public void MainMenuButton()
    {
        GameManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(4, 1);
    }
}
