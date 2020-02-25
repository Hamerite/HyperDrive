﻿//Created by Dylan LeClair
//Last revised 24-02-20 (Dylan LeClair)

using System.Collections;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    [SerializeField]
    new ParticleSystem particleSystem;

    GameManager gameManager;

    readonly WaitForSeconds timer = new WaitForSeconds(1.0f);

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!particleSystem.IsAlive())
            StartCoroutine(DelaySceneChange());
    }

    IEnumerator DelaySceneChange()
    {
        yield return timer;

        gameManager.TraverseScenes(2, 3);
    }
}
