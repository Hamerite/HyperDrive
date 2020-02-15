using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    GameManager gameManager;

    readonly WaitForSeconds timer = new WaitForSeconds(4.5f);

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(MoveToMenu());
    }

    void Update()
    {
        if (Input.anyKeyDown)
            gameManager.TraverseScenes(0, 1);
    }

    IEnumerator MoveToMenu()
    {
        yield return timer;
        gameManager.TraverseScenes(0, 1);
    }
}
