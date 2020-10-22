//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)

using UnityEngine;
using UnityEngine.UI;

public class Music_ButtonController : MonoBehaviour
{
    [SerializeField] MusicPlayer musicPlayer;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button[] songPlayButtons;
    [SerializeField] ScrollRect scrollRect;

    bool canScroll = true;
    int index;
    float verticalPos;

    private void Start()
    {
        if (!Cursor.visible)
            songPlayButtons[index].Select();

        verticalPos = 1.0f;

        foreach (Button item in songPlayButtons)
            item.onClick.AddListener(() => PlayTrack(System.Array.IndexOf(songPlayButtons, item)));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            GameManager.Instance.TraverseScenes(4, 1);

        if(!Cursor.visible && Input.GetAxis("Vertical") > 0.0f ^ Input.GetAxis("Vertical") < 0.0f
            && canScroll)
        {
            canScroll = false;
            Invoke(nameof(DelayScroll), 0.35f);

            if (Input.GetAxis("Vertical") > 0.0f)
                index = Mathf.Clamp(index - 1, 0, songPlayButtons.Length - 1);
            else
                index = Mathf.Clamp(index + 1, 0, songPlayButtons.Length);

            if (index == 9)
            {
                mainMenuButton.Select();
                MenusManager.Instance.SetSelectedButton(mainMenuButton, null);
            }
            else
            {
                songPlayButtons[index].Select();
                MenusManager.Instance.SetSelectedButton(songPlayButtons[index], null);
            }

            verticalPos = 1.0f - ((float)index / (songPlayButtons.Length - 1));
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, verticalPos, Time.deltaTime * 20.0f);
        }
    }

    void DelayScroll()
    {
        canScroll = true;
    }

    public void PlayTrack(int index)
    {
        AudioManager.Instance.PlayButtonSound(1);
        musicPlayer.PlaySong(index);
    }

    public void ButtonSelected()
    {
        AudioManager.Instance.PlayButtonSound(0);
    }

    public void MainMenuButton()
    {
        AudioManager.Instance.PlayButtonSound(1);
        GameManager.Instance.TraverseScenes(4, 1);
    }
}
