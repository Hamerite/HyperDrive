using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyManager : MonoBehaviour
{
    public static S2_EnemyManager Instance { private set; get; }
    [SerializeField] protected List<Transform> spawnPoint = new List<Transform>();
    [SerializeField] protected List<S2_EnemyStats> enemies = new List<S2_EnemyStats>();
    //private Dictionary<Vector3, bool> planePositions = new Dictionary<Vector3, bool>();
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
       // ClearAndPopulatePlanePositions();
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
        //ClearAndPopulatePlanePositions();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
            {
                Vector3 nextPoint = ChooseNearest(enemies[i].transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
                //planePositions[nextPoint] = true;
                enemies[i].SetTarget(nextPoint);

                //else
                //    enemies[i].SetTarget(nextPoint.transform.position - new Vector3(0,1,0));
            }
        }
        Invoke(nameof(SetNewTarget), S2_PoolController.Instance.GetWaitTime());
    }

    //void ClearAndPopulatePlanePositions()
    //{
    //    if (planePositions.Count > 0) planePositions.Clear();
    //    for (int i = 0; i < S2_PointsPlanesCheckIn.Instance.ReturnPointPlanes().Count; i++) { planePositions.Add(S2_PointsPlanesCheckIn.Instance.ReturnPointPlanes()[i], false); }
    //}

    Vector3 ChooseNearest(Vector3 location, List<Vector3> destinations)
    {
        float nearestSqrMag = float.PositiveInfinity;
        Vector3 nearestVector3 = Vector3.zero;

        foreach (Vector3 item in destinations)
        {
            float sqrMag = (item - location).sqrMagnitude;

            if (sqrMag < nearestSqrMag)
            {
                nearestSqrMag = sqrMag;
                nearestVector3 = item;
            }
        }
        return nearestVector3;
    }
}
