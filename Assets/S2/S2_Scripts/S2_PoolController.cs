using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PoolController : MonoBehaviour
{
    GameManager gameManager;

    WaitForSeconds spawnTimer;

    readonly WaitForSeconds LimitTimer = new WaitForSeconds(2.5f);
    readonly List<GameObject>[] asteroids = new List<GameObject>[3];
    readonly List<GameObject>[] asteroidsArray = new List<GameObject>[5];

    bool hasChanged;
    float speed = 75.0f;
    float waitTime = 2.0f;

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

        spawnTimer = new WaitForSeconds(waitTime);
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
        if (gameManager.GetNumbers("Counter") % 15 == 0 && gameManager.GetNumbers("Counter") != 0 && !hasChanged)
        {
            if (gameManager.GetNumbers("Counter") % 30 == 0 && gameManager.GetNumbers("Counter") != 90 && 
                gameManager.GetNumbers("Counter") != 180 && gameManager.GetNumbers("Counter") < 301)
            {
                speed += 5.0f;
            }
            else if (gameManager.GetNumbers("Counter") != 90 && gameManager.GetNumbers("Counter") != 180 &&
                    gameManager.GetNumbers("Counter") < 286)
                 {
                     waitTime -= 0.1f;
                 }
            if (gameManager.GetNumbers("Counter") == 90)
            {
                for (int i = 0; i < asteroids[1].Count - 1; i++)
                {
                    asteroidsArray[i].Clear();
                    asteroidsArray[i].Add(asteroids[1][i]);
                }
            }
            if (gameManager.GetNumbers("Counter") == 180)
            {
                for (int i = 0; i < asteroids[1].Count - 1; i++)
                {
                    asteroidsArray[i].Clear();
                    asteroidsArray[i].Add(asteroids[2][i]);
                }
            }

            hasChanged = true;
            StartCoroutine(LimitChange());
        }
    }

    #region Object Pooler
    IEnumerator ChooseObstacle()
    {
        int RNG = Random.Range(0, asteroidsArray.Length - 1);
        GameObject newObstacle = GetObstacle(RNG);

        newObstacle.transform.position = transform.position;
        newObstacle.SetActive(true);

        yield return spawnTimer;

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
    #endregion

    #region Gameplay Functions
    public void ResetArrays()
    {
        for (int i = 0; i < asteroids[2].Count - 1; i++)
        {
            asteroidsArray[i].Clear();
            asteroidsArray[i].Add(asteroids[0][i]);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetDifficultyPerameters()
    {
        waitTime = 2.0f;
    }

    IEnumerator LimitChange()
    {
        yield return LimitTimer;
        hasChanged = false;
    }
    #endregion
}