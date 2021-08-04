//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyStats : MonoBehaviour {
    [SerializeField] protected S2_EnemyBaseClass stats = null;

    GameObject target;

    bool spawned = false;

    void Start() { Invoke(nameof(GetTarget), 1f); }

    void Update() {
        if (!spawned) return;
        if (transform.position.z > target.transform.position.z) { GetTarget(); }

        Vector3 position = new Vector3(target.transform.position.x, target.transform.position.y, 10);
        transform.position = Vector3.MoveTowards(transform.position, position, stats.GetMaxSpeed() * Time.deltaTime);
    }

    void GetTarget() {
        spawned = true;
        target = ChooseNearest(transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
    }

    GameObject ChooseNearest(Vector3 location, List<GameObject> destinations) {
        float nearestSqrMag = float.PositiveInfinity;
        GameObject nearestVector3 = null;

        foreach (GameObject item in destinations) {
            float sqrMag = (item.transform.position - location).sqrMagnitude;

            if(sqrMag < nearestSqrMag) {
                nearestSqrMag = sqrMag;
                nearestVector3 = item;
            }
        }

        return nearestVector3;
    }

    public void Shoot()
    {

    }
}
