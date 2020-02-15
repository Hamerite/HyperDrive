using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_AsteroidController : MonoBehaviour
{
    GameManager gameManager;
    S2_PoolController s2_PoolController;

    Transform[] asteroids;

    GameObject destroyer;

    readonly List<Vector3> rotateDir =  new List<Vector3>();
    readonly List<float> rotateSpeed =  new List<float>();

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        s2_PoolController = FindObjectOfType<S2_PoolController>();

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
            transform.position = Vector3.MoveTowards(transform.position, destroyer.transform.position, 1.0f * s2_PoolController.GetSpeed() * Time.deltaTime);

        for (int i = 1; i < asteroids.Length - 4; i++)
             asteroids[i].Rotate(rotateDir[i] * rotateSpeed[i]);

        if (transform.position == destroyer.transform.position)
        {
            gameObject.SetActive(false);
            gameManager.SetNumbers("Counter", 0);
        }
    }
}