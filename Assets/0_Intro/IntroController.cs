//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)
using UnityEngine;

public class IntroController : MonoBehaviour {
    [SerializeField] CanvasGroup canvasGroup = null;

    void Start() { LogoFadeIn(); }

    void Update() { if (Input.anyKeyDown) GameManager.Instance.TraverseScenes(1); }

    void LogoFadeIn() { canvasGroup.LeanAlpha(1.0f, 7.0f).setOnComplete(() => GameManager.Instance.TraverseScenes(1)); }
}
