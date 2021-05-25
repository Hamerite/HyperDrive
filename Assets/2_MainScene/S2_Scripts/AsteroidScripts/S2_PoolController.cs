//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AsteroidLevels {
    public string name = null;
    public AsteroidArrays[] asteroidArrays;
}

[System.Serializable]
public class AsteroidArrays {
    public string name = null;
    public GameObject asteroids;
}

public class S2_PoolController : MonoBehaviour {
    public static S2_PoolController Instance { get; private set; }

    [SerializeField] AsteroidLevels[] asteroidLevels = null;
    readonly List<GameObject>[] inUse = new List<GameObject>[7];
    readonly List<int> benched = new List<int>();

    int RNG;
    int arrayIndex = 0;
    float speed = 75.0f;
    float waitTime = 1.8f;
    float benchedTime = 5.4f;

    readonly string[] obstacleDifficulty = { "Very Easy", "Easy", "Medium", "Hard", "Very Hard" };

    private void Awake() {
        Instance = this;

        for (int i = 0; i < inUse.Length; i++) {
            inUse[i] = new List<GameObject>();
            inUse[i].TrimExcess();
        }
    }

    void Start() {
        GameManager.Instance.SetLevel(obstacleDifficulty[arrayIndex]);
        arrayIndex++;

        InvokeRepeating(nameof(ChooseObstacle), 0.5f, waitTime);
        benched.TrimExcess();
    }

    void ChooseObstacle() {
        RNG = Random.Range(0, asteroidLevels[arrayIndex].asteroidArrays.Length);
        if (benched.Contains(RNG)) {
            CancelInvoke(nameof(ChooseObstacle));
            InvokeRepeating(nameof(ChooseObstacle), 0.0f, waitTime);
            return;
        }
        else {
            benched.Add(RNG);
            GameObject newObstacle = GetObstacle(RNG);

            bool flipVertical = Random.value > 0.485f;
            bool flipHorizontal = Random.value > 0.485f;

            newObstacle.transform.position = transform.position;
            Quaternion rotation = newObstacle.transform.rotation;
            if (flipVertical) rotation.x = 180;
            if (flipHorizontal) rotation.y = 180;
            newObstacle.SetActive(true);

            InvokeRepeating(nameof(BringBackBenched), benchedTime, benchedTime);
        }
    }

    GameObject GetObstacle(int index) {
        for (int i = 0; i < inUse[index].Count; i++) if (!inUse[index][i].activeInHierarchy) return inUse[index][i];

        GameObject obstacle = Instantiate(asteroidLevels[arrayIndex - 1].asteroidArrays[index].asteroids);
        obstacle.SetActive(false);
        inUse[index].Add(obstacle);
        return obstacle;
    }

    void BringBackBenched() { if (benched.Count != 0) benched.RemoveAt(0); }

    public void CheckForBehaviourChange() {
        int counterValue = GameManager.Instance.GetNumbers("Counter");

        if (counterValue % 90 == 0) {
            for (int i = 0; i < inUse.Length; i++) inUse[i].Clear();

            benched.Clear();
            GameManager.Instance.SetLevel(obstacleDifficulty[arrayIndex]);
            arrayIndex++;
        }
        else if (counterValue % 30 == 0) speed += 5.0f;
        else if (counterValue % 15 == 0) {
            CancelInvoke(nameof(ChooseObstacle));
            CancelInvoke(nameof(BringBackBenched));

            waitTime -= 0.1f;
            benchedTime -= 0.3f;

            InvokeRepeating(nameof(ChooseObstacle), waitTime + 0.1f, waitTime);
        }
    }

    public float GetSpeed() { return speed; }
}