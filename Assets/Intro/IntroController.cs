//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)

using UnityEngine;

public class IntroController : MonoBehaviour
{
    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        canvasGroup.LeanAlpha(1.0f, 7.0f).setOnComplete(() => GameManager.Instance.TraverseScenes(0, 1));

        if (Input.anyKeyDown)
            GameManager.Instance.TraverseScenes(0, 1);
    }
}
