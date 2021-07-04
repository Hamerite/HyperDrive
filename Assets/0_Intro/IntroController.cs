//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)
using UnityEngine;

public class IntroController : MonoBehaviour {
    [SerializeField] protected CanvasGroup canvasGroup = null;

    void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Start() { LogoFadeIn(); }

    void Update() { if (Input.anyKeyDown) GameManager.Instance.TraverseScenes("StartScreen"); }

    void LogoFadeIn() { canvasGroup.LeanAlpha(1.0f, 7.0f).setOnComplete(() => GameManager.Instance.TraverseScenes("StartScreen")); }
}
