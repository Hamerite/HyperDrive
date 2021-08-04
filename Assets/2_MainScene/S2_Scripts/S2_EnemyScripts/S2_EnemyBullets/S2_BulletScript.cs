using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BulletScript : MonoBehaviour
{
    public float shotSpeed, shotDamage;
    [SerializeField] bool fired;

    private void OnEnable()
    {
        Invoke(nameof(DestroyBullet), 5);
        fired = true;
        S2_EnemyBulletPooler.Instance.AddBulletToInUse(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (fired)
            transform.position += transform.forward * shotSpeed * Time.deltaTime;
        else return;
    }

    public void DestroyBullet()
    {
        fired = false;
        transform.position = S2_EnemyBulletPooler.Instance.transform.position;
        S2_EnemyBulletPooler.Instance.RemoveBulletFromInUse(this);
        print("return bullet to bullet pool");
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroyBullet();
    }
}
