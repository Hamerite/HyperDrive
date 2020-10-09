﻿//Created by Dylan LeClair
//Last revised 13-09-20 (Dylan LeClair)

using System.Collections.Generic;
using UnityEngine;

public class S2_PlayerController : MonoBehaviour
{
    Transform shipModel;

    readonly List<GameObject> shipChoice = new List<GameObject>();

    int index;

    void Awake()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            shipChoice.Add(item);
            item.SetActive(false);
        }
        shipChoice.TrimExcess();
    }

    void Start()
    {
        index = PlayerPrefs.GetInt("Selection");
        shipChoice[index].SetActive(true);
        shipModel = shipChoice[index].transform;
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical") * 12.0f * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * 12.0f * Time.deltaTime;
        transform.Translate(horizontal, vertical, 0.0f);

        float angleV = Input.GetAxis("Vertical") * 700.0f * Time.deltaTime;
        float angleH = Input.GetAxis("Horizontal") * 700.0f * Time.deltaTime;
        shipModel.rotation = Quaternion.RotateTowards(shipModel.rotation, Quaternion.Euler(-angleV, 0, -angleH), 700.0f);
    }

    public void PlayerDeath()
    {
        shipModel.gameObject.SetActive(false);
    }
}