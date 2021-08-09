using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyManager : MonoBehaviour
{

    [SerializeField] protected List<Transform> spawnPoint = new List<Transform>();
    [SerializeField] protected List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(false);
        }
            Invoke(nameof(SpawnWave), 1);
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = spawnPoint[Random.Range(0, spawnPoint.Count)].position;
            enemies[i].SetActive(true);
            enemies[i].GetComponent<S2_EnemyStats>().GetTarget();
        }
    }
}
