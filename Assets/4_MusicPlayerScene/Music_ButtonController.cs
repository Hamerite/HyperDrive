//Created by Dylan LeClair
//Last revised 20-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class Music_ButtonController : MonoBehaviour {
    [SerializeField] protected MusicPlayer musicPlayer = null;
    [SerializeField] protected Button mainMenuButton = null;
    [SerializeField] protected Button[] songPlayButtons = null;
    [SerializeField] protected ScrollRect scrollRect = null;

    protected bool canScroll = true;
    protected int index;
    protected float verticalPos;

    void Start() {
        if (!Cursor.visible) songPlayButtons[index].Select();

        verticalPos = 1.0f;
        foreach (Button item in songPlayButtons) item.onClick.AddListener(() => PlayTrack(System.Array.IndexOf(songPlayButtons, item)));
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) GameManager.Instance.TraverseScenes("StartScreen");

        if (Cursor.visible || !canScroll) return;
        if(Input.GetAxis("Vertical") > 0.0f ^ Input.GetAxis("Vertical") < 0.0f) {
            canScroll = false;
            Invoke(nameof(DelayScroll), 0.35f);

            if (Input.GetAxis("Vertical") > 0.0f) index = Mathf.Clamp(index - 1, 0, songPlayButtons.Length - 1);
            else index = Mathf.Clamp(index + 1, 0, songPlayButtons.Length);

            if (index == 9) {
                mainMenuButton.Select();
                MenusManager.Instance.SetSelectedButton(mainMenuButton, null);
            }
            else {
                songPlayButtons[index].Select();
                MenusManager.Instance.SetSelectedButton(songPlayButtons[index], null);
            }

            verticalPos = 1.0f - ((float)index / (songPlayButtons.Length - 1));
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, verticalPos, Time.deltaTime * 20.0f);
        }
    }

    void DelayScroll() { canScroll = true; }

    public void PlayTrack(int index) {
        AudioManager.Instance.PlayInteractionSound(1);
        musicPlayer.PlaySong(index);
    }

    public void ButtonSelected() {
        AudioManager.Instance.PlayInteractionSound(0);
    }

    public void MainMenuButton() {
        AudioManager.Instance.PlayInteractionSound(1);
        GameManager.Instance.TraverseScenes("StartScreen");
    }
}
