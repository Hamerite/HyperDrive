using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyManager : MonoBehaviour
{
    public static S2_EnemyManager Instance { private set; get; }
    [SerializeField] protected List<Transform> spawnPoint = new List<Transform>();
    [SerializeField] protected List<S2_EnemyStats> enemies = new List<S2_EnemyStats>();
    [SerializeField] protected List<S2_EnemyStats> enemiesInWave = new List<S2_EnemyStats>();

    private int
        waveCount, waveTime,
        topUpTime, upgradeTime,
        enemyCount, numberOfLiveEnemies,
        enemiesKilled, enemiesKilledTotal, 
        currentEnemyHealth, totalEnemyHealth,
        scaleDifficulty;

    private float 
        healthCache, currentShields,
        shieldCache, currentHealth;
    private bool waveComplete = false;
    private string difficulty;
    public void RemoveFromEnemiesInWave(S2_EnemyStats x)
    {
        enemiesInWave.Remove(x);
    }

    void UpgradeEnemies()
    {
        if (!waveComplete)
        {
            for (int i = 0; i < numberOfLiveEnemies; i++)
            {
                enemiesInWave[i].Upgrade();
            }
        }
    }

    void TopUpEnemies()
    {
        if (!waveComplete)
        {
            for (int i = 0; i < numberOfLiveEnemies; i++)
            {
                enemiesInWave[i].TopUp();
            }
        }
    }

    public void SetDifficulty(string x)
    {
        difficulty = x;
    }

    void Start()
    {
        Instance = this;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }
            Invoke(nameof(SpawnWave), 1);
    }
  
    public void SetEnemiesLive()
    {
        enemiesKilledTotal++;
        enemiesKilled++;
        numberOfLiveEnemies--;
    }

    public int GetEnemiesLive()
    {
        return numberOfLiveEnemies;
    }

    void GetPlayerShieldsAndHealth()
    {
      shieldCache = S2_HUDUI.Instance.GetAttributesValue(0);
      healthCache = S2_HUDUI.Instance.GetAttributesValue(1);
    }

    float HealthScoring()
    {
        currentHealth = S2_HUDUI.Instance.GetAttributesValue(1);
        return currentHealth / healthCache;
    } 

    float ShieldScoring()
    {
        currentShields = S2_HUDUI.Instance.GetAttributesValue(0);
        return currentShields / shieldCache;
    }
    void AdaptiveGameplay()
    {
        if (HealthScoring() >= 0.75) scaleDifficulty++;
        else if (HealthScoring() <= 25) scaleDifficulty--;

        if (ShieldScoring() >= 0.75) scaleDifficulty++;
        else if (ShieldScoring() <= 25) scaleDifficulty--;
    }

    void SpawnWave()
    {
        waveComplete = false;
        if(waveCount > 0)
        {

        }
        for (int i = 0; i < enemyCount; i++)
        {
            switch (difficulty)
            {
                case "Very Easy":
                    enemyCount = 3; waveTime = 3;topUpTime = 5;upgradeTime = 9;
                    break;
                case "Easy":
                    enemyCount = 3; waveTime = 2; topUpTime = 4; upgradeTime = 8;

                    break;
                case "Medium":
                    enemyCount = 3; waveTime = 2; topUpTime = 3; upgradeTime = 7;

                    break;
                case "Hard":
                    enemyCount = 2; waveTime = 1; topUpTime = 2; upgradeTime = 6;

                    break;
                case "Very Hard":
                    enemyCount = 1; waveTime = 1; topUpTime = 1; upgradeTime = 5;

                    break;
                default:
                    break;
            }
        }
        Invoke(nameof(TopUpEnemies), topUpTime);
        Invoke(nameof(UpgradeEnemies), upgradeTime);
        waveCount++;
    }
    void SpawnEnemy(int x)
    {
        S2_EnemyStats newEnemy = Instantiate(enemies[x], spawnPoint[Random.Range(0, spawnPoint.Count)].position, Quaternion.identity);
        newEnemy.gameObject.SetActive(true);
        newEnemy.SetTarget(ChooseNearest(newEnemy.transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext()));
        totalEnemyHealth += newEnemy.GetMaxHealth();
        Invoke(nameof(SetNewTarget), S2_PoolController.Instance.GetWaitTime());
    }

    public void EndWave()
    {
        waveComplete = true;
        totalEnemyHealth = 0;
        CancelInvoke();
        GetPlayerShieldsAndHealth();
    }

     void SetNewTarget()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemiesInWave[i].isActiveAndEnabled)
            {
                GameObject nextPoint = ChooseNearest(enemiesInWave[i].transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
                enemiesInWave[i].SetTarget(nextPoint);
                enemiesInWave[i].GetComponent<S2_EnemyStats>().isMoving = true;
            }
        }
        Invoke(nameof(SetNewTarget), S2_PoolController.Instance.GetWaitTime());
    }

    public GameObject ChooseNearest(Vector3 location, List<GameObject> destinations)
    {
        float nearestSqrMag = float.PositiveInfinity;
        GameObject nearestVector3 = null;

        foreach (GameObject item in destinations)
        {
            float sqrMag = (item.transform.position - location).sqrMagnitude;

            if (sqrMag < nearestSqrMag)
            {
                nearestSqrMag = sqrMag;
                nearestVector3 = item;
            }
        }
        return nearestVector3;
    }
}
