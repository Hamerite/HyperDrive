//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
//Modified 10/20/21 (Kyle Ennis)
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class AsteroidArrays {
    public string name = null;
    public GameObject asteroidsPrefab;
    public List<GameObject> inSceneBank = new List<GameObject>();
}

[System.Serializable] public class AsteroidLevels {
    public string name = null;
    public AsteroidArrays[] asteroidArrays;
}

public class S2_PoolController : MonoBehaviour {
    public static S2_PoolController Instance { get; private set; }

    [SerializeField] protected AsteroidLevels[] asteroidLevels = null;
    protected readonly List<int> benched = new List<int>();

    protected GameObject startObstalce;

    protected int RNG, arrayIndex;
    protected float speed = 75.0f, waitTime = 1.8f, benchedTime = 5.4f;
    public float GetWaitTime()
    {
        return waitTime;
    }
    protected readonly string[] obstacleDifficulty = { "Very Easy", "Easy", "Medium", "Hard", "Very Hard" };

    void Awake() {
        Instance = this;
        for (int i = 0; i < asteroidLevels.Length; i++) for (int j = 0; j < asteroidLevels[arrayIndex].asteroidArrays.Length; j++) asteroidLevels[i].asteroidArrays[j].inSceneBank.TrimExcess();
    }

    void Start() {
        S2_HUDUI.Instance.SetLevel(obstacleDifficulty[arrayIndex]);
        arrayIndex++;

        InvokeRepeating(nameof(ChooseObstacle), 3.5f, waitTime);
        benched.TrimExcess();
    }

    void ChooseObstacle() {
        RNG = Random.Range(0, asteroidLevels[arrayIndex].asteroidArrays.Length);
        if (benched.Contains(RNG)) {
            CancelInvoke(nameof(ChooseObstacle));
            InvokeRepeating(nameof(ChooseObstacle), 0, waitTime);
            return;
        }
        else {
            benched.Add(RNG);
            GameObject newObstacle = GetObstacle(RNG);

            newObstacle.transform.position = transform.position;
            Quaternion rotation = newObstacle.transform.rotation;
            if (Random.value > 0.485f) rotation.x = 180;
            if (Random.value > 0.485f) rotation.y = 180;

            newObstacle.SetActive(true);
            InvokeRepeating(nameof(BringBackBenched), benchedTime, benchedTime);
        }
    }

    GameObject GetObstacle(int index) {
        for (int i = 0; i < asteroidLevels[arrayIndex - 1].asteroidArrays[index].inSceneBank.Count; i++) if (!asteroidLevels[arrayIndex - 1].asteroidArrays[index].inSceneBank[i].activeInHierarchy) return asteroidLevels[arrayIndex - 1].asteroidArrays[index].inSceneBank[i];

        GameObject obstacle = Instantiate(asteroidLevels[arrayIndex - 1].asteroidArrays[index].asteroidsPrefab);
        asteroidLevels[arrayIndex - 1].asteroidArrays[index].inSceneBank.Add(obstacle);
        return obstacle;
    }

    void BringBackBenched() {
        if (benched.Count <= 0) return;
        benched.RemoveAt(0); 
    }

    public void CheckForBehaviourChange() {
        int counterValue = S2_HUDUI.Instance.GetObstacleCounter();

        if (counterValue % 90 == 0) {
            benched.Clear();
            S2_HUDUI.Instance.SetLevel(obstacleDifficulty[arrayIndex]);
            S2_EnemyManager.Instance.SetDifficulty(obstacleDifficulty[arrayIndex-1]);
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
        else if(counterValue % 10 == 0)
        {
            S2_EnemyManager.Instance.AdaptiveGameplay();
            S2_EnemyManager.Instance.GetPlayerShieldsAndHealth();
            S2_EnemyManager.Instance.ResetEnemyCounter();
        }
    }

    public float GetSpeed() { return speed; }
}