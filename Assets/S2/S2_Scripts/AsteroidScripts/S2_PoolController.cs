//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)

using System.Collections.Generic;
using UnityEngine;

public class S2_PoolController : MonoBehaviour
{
    public static S2_PoolController Instance { get; private set; }

    readonly List<GameObject>[] asteroids = new List<GameObject>[5];
    readonly List<GameObject>[] asteroidsArray = new List<GameObject>[7];
    readonly List<int> benched = new List<int>();

    int RNG;
    int arrayIndex = 0;
    int index = 0;
    float speed = 75.0f;
    float waitTime = 1.8f;
    float benchedTime = 5.4f;

    readonly string[] obstacleDifficulty = { "Very Easy", "Easy", "Medium", "Hard", "Very Hard" };

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < asteroids.Length; i++)
        {
            asteroids[i] = new List<GameObject>();
        }

        foreach (string tag in obstacleDifficulty)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag(tag))
            {
                asteroids[index].Add(item);
                item.SetActive(false);
            }
            index++;
        }

        for (int i = 0; i < asteroids.Length; i++)
        {
            asteroids[i].TrimExcess();
        }
    }

    void Start()
    {
        GameManager.Instance.SetLevel(obstacleDifficulty[arrayIndex]);
        arrayIndex++;

        for (int i = 0; i < asteroids[0].Count; i++)
        {
            asteroidsArray[i] = new List<GameObject>()
            {
                asteroids[0][i]
            };

            asteroidsArray[i].TrimExcess();
        }

        InvokeRepeating(nameof(ChooseObstacle), 0.5f, waitTime);
        benched.TrimExcess();
    }

    void ChooseObstacle()
    {
        RNG = Random.Range(0, asteroidsArray.Length);
        if (benched.Contains(RNG))
        {
            CancelInvoke(nameof(ChooseObstacle));
            InvokeRepeating(nameof(ChooseObstacle), 0.0f, waitTime);
            return;
        }
        else
        {
            benched.Add(RNG);
            GameObject newObstacle = GetObstacle(RNG);

            bool flipVertical = Random.value > 0.485f;
            bool flipHorizontal = Random.value > 0.485f;

            newObstacle.transform.position = transform.position;
            Quaternion rotation = newObstacle.transform.rotation;
            if (flipVertical)
                rotation.x = 180;
            if (flipHorizontal)
                rotation.y = 180;
            newObstacle.SetActive(true);

            InvokeRepeating(nameof(BringBackBenched), benchedTime, benchedTime);
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

    void BringBackBenched()
    {
        if(benched.Count != 0)
            benched.RemoveAt(0);
    }

    public void CheckForBehaviourChange()
    {
        int counterValue = GameManager.Instance.GetNumbers("Counter");

        if (counterValue % 90 == 0)
        {
            benched.Clear();
            for (int i = 0; i < asteroids[1].Count - 1; i++)
                asteroidsArray[i].Clear();

            GameManager.Instance.SetLevel(obstacleDifficulty[arrayIndex]);

            for (int i = 0; i < asteroids[1].Count - 1; i++)
            {
                asteroidsArray[i].Add(asteroids[arrayIndex][i]);
                asteroidsArray[i].TrimExcess();
            }

            arrayIndex++;
        }
        else if (counterValue % 30 == 0)
            speed += 5.0f;
        else if (counterValue % 15 == 0)
        {
            CancelInvoke(nameof(ChooseObstacle));
            CancelInvoke(nameof(BringBackBenched));

            waitTime -= 0.1f;
            benchedTime -= 0.3f;

            InvokeRepeating(nameof(ChooseObstacle), waitTime + 0.1f, waitTime);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }
}