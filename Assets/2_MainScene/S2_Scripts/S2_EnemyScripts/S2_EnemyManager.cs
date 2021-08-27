using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyManager : MonoBehaviour
{
    public static S2_EnemyManager Instance { private set; get; }
    [SerializeField] protected List<Transform> spawnPoint = new List<Transform>();
    [SerializeField] protected List<S2_EnemyStats> enemies = new List<S2_EnemyStats>();
    private bool value = false;
    void Start()
    {
        Instance = this;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }
            Invoke(nameof(SpawnWave), 1);
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = spawnPoint[Random.Range(0, spawnPoint.Count)].position;
            enemies[i].gameObject.SetActive(true);
            enemies[i].SetTarget(ChooseNearest(enemies[i].transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext()));      
        }
        Invoke(nameof(SetNewTarget), S2_PoolController.Instance.GetWaitTime());
    }

     void SetNewTarget()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
            {
                GameObject nextPoint = ChooseNearest(enemies[i].transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
                enemies[i].SetTarget(nextPoint);
                enemies[i].GetComponent<S2_EnemyStats>().isMoving = true;
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
