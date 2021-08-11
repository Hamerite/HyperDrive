//Created by Dylan LeClair 11/08/21
//Last modified 11/08/21 (Dylan LeClair)
using UnityEngine;
using TMPro;

public class FeedbackMessageController : MonoBehaviour {
    public static FeedbackMessageController Instance { get; private set; }

    [SerializeField] protected CanvasGroup canvasGroup = null;
    [SerializeField] protected TextMeshProUGUI textmesh = null;

    void Awake() { Instance = this; }

    public void SetMessage(string message, Color color) {
        textmesh.text = message;
        textmesh.color = color;
        textmesh.fontSharedMaterial.SetColor("_GlowColor", color);
        canvasGroup.alpha = 1;

        FadeMessage();
    }

    void FadeMessage() { canvasGroup.LeanAlpha(0f, 1f); }
}
