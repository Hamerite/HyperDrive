//Created by Dylan LeClair
//Last revised 22-02-20 (Dylan LeClair)

using System.Collections;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    GameManager gameManager;
    CanvasGroup canvasGroup;

    readonly WaitForSeconds timer = new WaitForSeconds(1.0f);

    bool startLogo;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(MoveToMenu());
    }

    void Update()
    {
        if(startLogo && canvasGroup.alpha < 1)
            canvasGroup.alpha += 0.2f * Time.deltaTime;
        if (canvasGroup.alpha == 1)
            StartCoroutine(MoveToMenu());

        if (Input.anyKeyDown || !startLogo && canvasGroup.alpha == 1)
            gameManager.TraverseScenes(0, 1);
    }

    IEnumerator MoveToMenu()
    {
        yield return timer;
        startLogo = !startLogo;
    }
}
