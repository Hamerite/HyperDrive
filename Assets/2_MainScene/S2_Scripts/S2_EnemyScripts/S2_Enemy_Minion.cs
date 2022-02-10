using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Enemy_Minion : S2_EnemyBaseClass
{
    public override void Shoot()
    {
        canShoot = false;
        S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = canonPosition.position;
        S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
        S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetType(0);
        S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetShotSpeed(20);
        S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);

        shotMultiple++;

        if (shotMultiple >= 1) Invoke(nameof(EndShot), 0.1f);
        else
        {
            Invoke(nameof(Shoot), 0.8f);
        }
    }

    private void FixedUpdate()
    {
        PlayerDetection();
    }
}
