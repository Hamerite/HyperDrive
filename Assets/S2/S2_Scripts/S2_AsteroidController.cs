using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_AsteroidController : MonoBehaviour
{
    GameManager gameManager;
    Transform[] asteroids;
    GameObject destroyer;

    readonly List<float> rotateSpeed =  new List<float>();
    readonly List<Vector3> rotateDir =  new List<Vector3>();

    float speed = 50.0f;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        asteroids = GetComponentsInChildren<Transform>();
        destroyer = GameObject.FindGameObjectWithTag("Destroyer");
    }

    void Start()
    {
        for (int i = 0; i < asteroids.Length - 4; i++)
        {
            rotateDir.Add(new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));
            rotateSpeed.Add(Random.Range(0.01f, 0.03f));
        }
    }

    void Update()
    {
        if (isActiveAndEnabled)
            transform.position = Vector3.MoveTowards(transform.position, destroyer.transform.position, 1.0f * speed * Time.deltaTime);

        for (int i = 1; i < asteroids.Length - 4; i++)
             asteroids[i].Rotate(rotateDir[i] * rotateSpeed[i]);

        if (transform.position == destroyer.transform.position)
        {
            gameObject.SetActive(false);
            gameManager.SetNumbers("Counter", 0);
        }

        if (gameManager.GetNumbers("Counter") != 0 && gameManager.GetNumbers("Counter") % 20 == 0)
            speed += 10;
    }
}