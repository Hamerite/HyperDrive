//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyStats : MonoBehaviour, IKillable
{
    [SerializeField] protected S2_EnemyBaseClass stats = null;
    public enum ShipClass { speeder, tank, bomber};
    public ShipClass myClass;

    GameObject target;
    public GameObject GetTargetPosition()
    {
        return target;
    }

    bool spawned = false, canShoot = false;
    float shotTime = 1.0f, zPos = 10;
    int shotMultiple = 0;
    private void OnEnable()
    {
        Invoke(nameof(GetTarget), 1f); Invoke(nameof(Shoot), 5);
    }

    void Update()
    {
        if (!spawned) return;
        if (transform.position.z > target.transform.position.z) { GetTarget(); }
        Vector3 position = new Vector3(target.transform.position.x, target.transform.position.y, 10);
        transform.position = Vector3.MoveTowards(transform.position, position, stats.GetMaxSpeed() * Time.deltaTime);
    }

    public void GetTarget()
    {
        spawned = true;
        switch (myClass)
        {
            case ShipClass.speeder:
                target = ChooseNearest(transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
                break;
            case ShipClass.tank:
                target = ChooseNearest(S2_PlayerController.Instance.transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
                break;
            case ShipClass.bomber:
                target = ChooseNearest(transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
                break;
            default:
                break;
        }
    }

    GameObject ChooseNearest(Vector3 location, List<GameObject> destinations)
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

    public void Shoot()
    {
        if (S2_EnemyBulletPooler.Instance.GetBullets().Count > 0)
        {
            switch (myClass)
            {
                case ShipClass.speeder:
                    canShoot = true;
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = transform.position;
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
                    canShoot = false;
                    shotMultiple++;
                    if (shotMultiple >= 3) Invoke(nameof(EndShot), 0.1f);
                    else
                        Invoke(nameof(Shoot), 0.3f);
                    break;
                case ShipClass.tank:
                    canShoot = true;
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = transform.position;
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
                    Invoke(nameof(EndShot), 0.1f);
                    break;
                case ShipClass.bomber:
                    canShoot = true;
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = transform.position;
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
                    Invoke(nameof(Shoot), 2f);
                    break;
                default:
                    break;
            }
        }
        else
            return;
    }

    void EndShot()
    {
        shotMultiple = 0;
        return;
    }
    public IEnumerator CheckHit(bool x)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Die()
    {
        throw new System.NotImplementedException();
    }


}
