//Created by Dylan LeClair
//Last revised 07-03-20 (Dylan LeClair)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PoolController : MonoBehaviour
{
    GameManager gameManager;

    readonly List<GameObject>[] asteroids = new List<GameObject>[3];
    readonly List<GameObject>[] asteroidsArray = new List<GameObject>[5];

    #region Object Spawn Variables
    Coroutine spawnRoutine;

    WaitForSeconds spawnTimer;
    WaitForSeconds benchedTimer;
    readonly WaitForSeconds LimitTimer = new WaitForSeconds(2.5f);

    readonly List<int> benched = new List<int>();

    bool hasChanged;
    bool needsRestart;
    int RNG;
    float speed = 75.0f;
    float waitTime = 1.8f;
    float benchedTime = 5.4f;
    #endregion

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
        benchedTimer = new WaitForSeconds(benchedTime);

        for (int i = 0; i < 2; i++)
        {
            asteroids[i].TrimExcess();
        }
    }

    void Start()
    {
        for (int i = 0; i < asteroids[0].Count; i++)
        {
            asteroidsArray[i] = new List<GameObject>()
            {
                asteroids[0][i]
            };

            asteroidsArray[i].TrimExcess();
        }
        spawnRoutine = StartCoroutine(ChooseObstacle());
        benched.TrimExcess();
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
                     benchedTime -= 0.3f;
                 }
            if (gameManager.GetNumbers("Counter") == 90)
            {
                for (int i = 0; i < asteroids[1].Count - 1; i++)
                {
                    asteroidsArray[i].Clear();
                    benched.Clear();

                    asteroidsArray[i].Add(asteroids[1][i]);
                    asteroidsArray[i].TrimExcess();
                }

            }
            if (gameManager.GetNumbers("Counter") == 180)
            {
                for (int i = 0; i < asteroids[1].Count - 1; i++)
                {
                    asteroidsArray[i].Clear();
                    benched.Clear();

                    asteroidsArray[i].Add(asteroids[2][i]);
                    asteroidsArray[i].TrimExcess();
                }
            }

            hasChanged = true;
            StartCoroutine(LimitChange());
        }

        if(needsRestart)
        {
            needsRestart = false;
            spawnRoutine = StartCoroutine(ChooseObstacle());
        }
    }

    #region Object Pooler
    IEnumerator ChooseObstacle()
    {
        RNG = Random.Range(0, asteroidsArray.Length);
        if(benched.Contains(RNG))
        {
            needsRestart = true;
            if(spawnRoutine != null)
                StopCoroutine(spawnRoutine);
        }
        else
        {
            benched.Add(RNG);
            GameObject newObstacle = GetObstacle(RNG);

            bool flipVertical = Random.value > 0.5f;
            bool flipHorizontal = Random.value > 0.5f;

            newObstacle.transform.position = transform.position;
            Quaternion rotation = newObstacle.transform.rotation;
            if (flipVertical)
                rotation.x = 180;
            if (flipHorizontal)
                rotation.y = 180;
            newObstacle.SetActive(true);

            yield return spawnTimer;

            spawnRoutine = StartCoroutine(ChooseObstacle());
            StartCoroutine(BringBackBenched());
        }
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

    IEnumerator BringBackBenched()
    {
        yield return benchedTimer;

        if(benched.Count != 0)
            benched.RemoveAt(0);

        StartCoroutine(BringBackBenched());
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

    IEnumerator LimitChange()
    {
        yield return LimitTimer;
        hasChanged = false;
    }
    #endregion
}