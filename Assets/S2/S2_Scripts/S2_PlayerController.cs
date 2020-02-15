﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PlayerController : MonoBehaviour
{
    [SerializeField]
    AudioClip[] s2PlayerClips;
    [SerializeField]
    ParticleSystem playerDeathParticles;

    GameManager gameManager;
    AudioSource audioSource;

    Transform shipModel;
    new Collider collider;

    readonly WaitForSeconds timer = new WaitForSeconds(0.25f);
    readonly List<GameObject> shipChoice = new List<GameObject>();

    readonly LayerMask[] layers = { 9, 10, 11, 12 };
    readonly Vector3[] dir = new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    bool isAlive;
    bool canScore = true;
    int index;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        collider = GetComponent<Collider>();

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            shipChoice.Add(item);
            item.SetActive(false);
        }
    }

    void Start()
    {
        index = PlayerPrefs.GetInt("Selection");
        shipChoice[index].SetActive(true);
        shipModel = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        audioSource.clip = s2PlayerClips[0];
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.Play();

        isAlive = true;
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical") * 12.0f * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * 12.0f * Time.deltaTime;
        transform.Translate(horizontal, vertical, 0.0f);

        float angleV = Input.GetAxis("Vertical") * 700.0f * Time.deltaTime;
        float angleH = Input.GetAxis("Horizontal") * 700.0f * Time.deltaTime;
        shipModel.rotation = Quaternion.RotateTowards(shipModel.rotation, Quaternion.Euler(-angleV, 0, -angleH), 700.0f);

        foreach (Vector3 item in dir)
        {
            if (Physics.Raycast(transform.position, item, out RaycastHit hit, 0.5f, 1 << 8))
                transform.Translate(hit.normal / 4.5f);
            if (Physics.Raycast(transform.position, item, 10.0f, 1 << 9))
                audioSource.PlayOneShot(s2PlayerClips[2]);
        }

        foreach (LayerMask item in layers)
        {
            if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents / 2, Vector3.forward, transform.rotation, 2.1f, 1 << item))
                if (isAlive && canScore)
                {
                    if (item == 9)
                    {
                        audioSource.clip = s2PlayerClips[1];
                        audioSource.Play();
                        Instantiate(playerDeathParticles, transform);

                        shipChoice[index].SetActive(false);
                        isAlive = false;

                        if (!playerDeathParticles.IsAlive())
                            gameManager.TraverseScenes(2, 3);
                    }
                    if (item == 10)
                        gameManager.SetNumbers("Score", 1);
                    if (item == 11)
                        gameManager.SetNumbers("Score", 2);
                    if (item == 12)
                        gameManager.SetNumbers("Score", 3);

                    canScore = false;
                    StartCoroutine(ScoreLimiter());
                }
        }

        IEnumerator ScoreLimiter()
        {
            yield return timer;

            canScore = true;
        }
    }
}