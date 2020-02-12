using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PoolController : MonoBehaviour
{
    GameManager gameManager;

    readonly List<GameObject>[] asteroids = new List<GameObject>[3];
    readonly List<GameObject>[] asteroidsArray = new List<GameObject>[5];

    readonly WaitForSeconds timer = new WaitForSeconds(2.0f);

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        for (int i = 0; i < asteroids.Length; i++)
        {
            asteroids[i] = new List<GameObject>();
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("EAsteroid"))
        {
            asteroids[0].Add(item);
            item.SetActive(false);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("MAsteroid"))
        {
            asteroids[1].Add(item);
            item.SetActive(false);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("HAsteroid"))
        {
            asteroids[2].Add(item);
            item.SetActive(false);
        }
    }

    void Start()
    {
        for (int i = 0; i < asteroids[0].Count - 1; i++)
        {
            asteroidsArray[i] = new List<GameObject>()
            {
                asteroids[0][i]
            };
        }

        StartCoroutine(ChooseObstacle());
    }

    void Update()
    {
        if(gameManager.GetCounter() != 0 && gameManager.GetCounter() == 30)
        {
            for (int i = 0; i < asteroids[1].Count - 1; i++)
            {
                asteroidsArray[i].Clear();
                asteroidsArray[i].Add(asteroids[1][i]);
            }
        }
        if (gameManager.GetCounter() != 0 && gameManager.GetCounter() == 60)
        {
            for (int i = 0; i < asteroids[2].Count - 1; i++)
            {
                asteroidsArray[i].Clear();
                asteroidsArray[i].Add(asteroids[2][i]);
            }
        }
    }

    IEnumerator ChooseObstacle()
    {
        int RNG = Random.Range(0, asteroidsArray.Length - 1);
        GameObject newObstacle = GetObstacle(RNG);

        newObstacle.transform.position = transform.position;
        newObstacle.SetActive(true);

        yield return timer;

        StartCoroutine(ChooseObstacle());
    }

    GameObject GetObstacle(int index)
    {
        for (int i = 0; i < asteroidsArray[index].Count; i++)
            if (!asteroidsArray[index][i].activeInHierarchy)
                return asteroidsArray[index][i];

        GameObject obstacle = Instantiate(asteroidsArray[index][0]);
        obstacle.SetActive(false);
        asteroidsArray[index].Add(obstacle);
        return obstacle;
    }

    public void ResetArrays()
    {
        for (int i = 0; i < asteroids[2].Count - 1; i++)
        {
            asteroidsArray[i].Clear();
            asteroidsArray[i].Add(asteroids[0][i]);
        }
    }
}