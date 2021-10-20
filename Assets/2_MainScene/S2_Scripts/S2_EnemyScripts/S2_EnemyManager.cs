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
        enemyCount = 3, numberOfLiveEnemies,
        enemiesKilled, enemiesKilledTotal,
        scaleDifficulty = 0;

    private int[]
        minion_Ve =  { 85, 75, 65 },
        speeder_Ve = { 15, 20, 25 },
        tank_Ve =    {  0,  4,  5 },
        bomber_Ve =  {  0,  1,  5 },

        minion_E =   { 70, 60, 40 },
        speeder_E =  { 25, 25, 35 },
        tank_E =     {  5, 10, 15 },
        bomber_E =   {  0,  5, 10 },

        minion_M =   { 40, 35, 30 },
        speeder_M =  { 40, 35, 30 },
        tank_M =     { 15, 20, 25 },
        bomber_M =   {  5, 10, 15 },

        minion_H =   { 20, 15, 10 },
        speeder_H =  { 50, 45, 35 },
        tank_H =     { 20, 25, 30 },
        bomber_H =   { 10, 15, 25 },
        
        minion_VH =   { 10,  5,  5 },
        speeder_VH =  { 35, 25, 15 },
        tank_VH =     { 35, 40, 45 },
        bomber_VH =   { 25, 30, 35 };

    private float 
        healthCache, currentShields,
        shieldCache, currentHealth,
        RNG;
    private bool waveComplete = false;
    private string difficulty = "Very Easy";

    void EnemySpawnDifficultyAdjuster(int difficultyScale)
    {
        int choice;
        if (difficultyScale <= -2) choice = 0;
        else if (difficultyScale >= 2) choice = 2;
        else choice = 1;

        RNG = Random.Range(0, 100);
        switch (difficulty)
        {
            case "Very Easy":
                if (RNG > minion_Ve[choice])
                {
                    if (RNG > (minion_Ve[choice] + speeder_Ve[choice]))
                    {
                        if (difficultyScale >= -1 && difficultyScale <= 1)
                        {
                            if (RNG > minion_Ve[choice] + speeder_Ve[choice] + tank_Ve[choice])
                                SpawnEnemy(3);
                            else
                                SpawnEnemy(2);
                        }
                        else
                            EnemySpawnDifficultyAdjuster(difficultyScale);
                    }
                    else
                        SpawnEnemy(1);
                }
                else
                    SpawnEnemy(0);
                break;
            case "Easy":
                if (RNG > minion_E[choice])
                {
                    if (RNG > (minion_E[choice] + speeder_E[choice]))
                    {

                        if (RNG > minion_E[choice] + speeder_E[choice] + tank_E[choice])
                        { 
                            if (difficultyScale >= -1 && difficultyScale <= 1) 
                                SpawnEnemy(3);
                            else
                                EnemySpawnDifficultyAdjuster(difficultyScale); 
                        }
                        else
                            SpawnEnemy(2);
                    }
                    else
                        SpawnEnemy(1);
                }
                else
                    SpawnEnemy(0);
                break;
            case "Medium":
                if (RNG > minion_M[choice] + speeder_M[choice])
                {
                        if (RNG > minion_M[choice] + speeder_M[choice] + tank_M[choice])
                            SpawnEnemy(3);
                        else
                            SpawnEnemy(2);
                }
                else
                {
                    int x = Random.Range(0, 100);
                    if (x <= 50)
                        SpawnEnemy(0);
                    else
                        SpawnEnemy(1);
                }
                break;
            case "Hard":
                if (RNG > speeder_H[choice])
                {

                    if (RNG > speeder_H[choice] + minion_H[choice] + tank_H[choice])
                        SpawnEnemy(3);
                    else
                    {
                        if (difficultyScale <= -2)
                        {
                            int x = Random.Range(0, 100);
                            if (x < 50) SpawnEnemy(0);
                            else SpawnEnemy(2);
                        }
                        else
                        {
                            if (RNG > speeder_H[choice] + minion_H[choice])
                                SpawnEnemy(2);
                            else
                                SpawnEnemy(0);
                        }    
                    }
                }
                else
                    SpawnEnemy(1);
                break;
            case "Very Hard":
                if (RNG > minion_VH[choice])
                {
                    if (RNG > (minion_VH[choice] + speeder_VH[choice]))
                    {

                        if (RNG > minion_VH[choice] + speeder_VH[choice] + tank_VH[choice])
                        {
                                SpawnEnemy(3);
                        }
                        else
                            SpawnEnemy(2);
                    }
                    else
                        SpawnEnemy(1);
                }
                else
                    SpawnEnemy(0);
                break;
            default:
                break;
        }
    }

    public void RemoveFromEnemiesInWave(S2_EnemyStats x)
    {
        enemiesInWave.Remove(x);
    }


    void Start()
    {
        Instance = this;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }
        Invoke(nameof(SpawnWave), 10);
        GetPlayerShieldsAndHealth();
    }

    void UpgradeEnemies()
    {
        if (!waveComplete)
        {
            if (numberOfLiveEnemies >= enemyCount)
            {
                for (int i = 0; i < numberOfLiveEnemies; i++)
                {
                    enemiesInWave[i].Upgrade();
                }
                Invoke(nameof(UpgradeEnemies), upgradeTime);
                print("Enemies upgraded");
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
            Invoke(nameof(TopUpEnemies), topUpTime);
            print("Enemy Health Restored");
        }
    }

    public void SetDifficulty(string x)
    {
        difficulty = x;
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

    public void GetPlayerShieldsAndHealth()
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
    public void AdaptiveGameplay()
    {
        if (HealthScoring() >= 0.75) scaleDifficulty++;
        else if (HealthScoring() <= 25) scaleDifficulty--;

        if (ShieldScoring() >= 0.75) scaleDifficulty++;
        else if (ShieldScoring() <= 25) scaleDifficulty--;

        if (enemiesKilled <= 4) scaleDifficulty--;
        else if (enemiesKilled >= 9) scaleDifficulty++;

        if (scaleDifficulty >= 3) scaleDifficulty = 3;
        else if (scaleDifficulty <= -3) scaleDifficulty = -3;

        print("Difficulty scale = " + scaleDifficulty);
    }

    void SpawnWave()
    {
        waveComplete = false;
        switch (difficulty)
        {
            case "Very Easy":
                enemyCount = 3; waveTime = 3; topUpTime = 5; upgradeTime = 9;
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
        for (int i = 0; i < enemyCount; i++)
        {
            EnemySpawnDifficultyAdjuster(scaleDifficulty);
            //SpawnEnemy(scaleDifficulty);
        }

        Invoke(nameof(TopUpEnemies), topUpTime);
        Invoke(nameof(UpgradeEnemies), upgradeTime);
        waveCount++;
    }
    void SpawnEnemy(int x)
    {
        S2_EnemyStats newEnemy = Instantiate(enemies[x], spawnPoint[Random.Range(0, spawnPoint.Count)].position, Quaternion.identity);
        enemiesInWave.Add(newEnemy);
        numberOfLiveEnemies++;
        newEnemy.gameObject.SetActive(true);
        newEnemy.SetTarget(ChooseNearest(newEnemy.transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext()));
        Invoke(nameof(SetNewTarget), S2_PoolController.Instance.GetWaitTime());
    }

    public void EndWave()
    {
        waveComplete = true;
        CancelInvoke();
        print("End Of Wave " + waveCount);
        Invoke(nameof(SpawnWave), waveTime);
    }

    public void ResetEnemyCounter()
    {
        enemiesKilled = 0;
    }

     void SetNewTarget()
    {
        if (enemyCount > 0)
        {
            for (int i = 0; i < enemiesInWave.Count; i++)
            {
                if (enemiesInWave[i].isActiveAndEnabled)
                {
                    GameObject nextPoint = ChooseNearest(enemiesInWave[i].transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
                    enemiesInWave[i].SetTarget(nextPoint);
                    enemiesInWave[i].GetComponent<S2_EnemyStats>().isMoving = true;
                }
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
