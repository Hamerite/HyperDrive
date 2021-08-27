using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BulletScript : MonoBehaviour
{
    [SerializeField] float shotSpeed = 5, shotDamage = 1;
    [SerializeField] GameObject[] bulletTypes;
    [SerializeField] bool fired;
    Vector3 startPos, endPos;
    int type;
    public void SetType(int x)
    {
        type = x;
    }
    public void SetShotSpeed(float x)
    {
        shotSpeed = x;
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Invoke(nameof(DestroyBullet), 5);
        fired = true;
        S2_EnemyBulletPooler.Instance.AddBulletToInUse(this);
        startPos = transform.position;
        endPos = PredictiveShot();

            for (int i = 0; i < bulletTypes.Length - 1; i++)
            {
                if (i == type) bulletTypes[type].gameObject.SetActive(true);
                else
                    bulletTypes[i].gameObject.SetActive(false);
            }


    }
    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            switch (type)
            {
                case 0:
                    transform.position += transform.forward * shotSpeed * Time.deltaTime;
                    break;
                case 1:
                    if ((transform.position.z - endPos.z) > 0.5f)
                        transform.position = Vector3.Lerp(transform.position, endPos, shotSpeed * Time.deltaTime);
                    else
                        transform.position = Vector3.Lerp(transform.position, new Vector3(endPos.x, endPos.y, endPos.z - 30), shotSpeed * Time.deltaTime);
                    break;
                case 2:
                    break;
                default:
                    break;
            }

        }
        else return;
    }
    Vector3 PredictiveShot()
    {
        Vector3 relativePosition = S2_PlayerController.Instance.transform.position;
        Vector3 supposedPosition = new Vector3(Input.GetAxis("Horizontal") * ShipStats.Instance.GetStats().GetMaxSpeed() * Time.deltaTime, Input.GetAxis("Vertical") * ShipStats.Instance.GetStats().GetMaxSpeed() * Time.deltaTime, S2_PlayerController.Instance.transform.position.z);

        return relativePosition - supposedPosition;
    }
    public void DestroyBullet()
    {
        fired = false;
        transform.position = S2_EnemyBulletPooler.Instance.transform.position;
        S2_EnemyBulletPooler.Instance.RemoveBulletFromInUse(this);
        for (int i = 0; i < bulletTypes.Length-1; i++)
        {
            if(bulletTypes[i].activeInHierarchy)
                bulletTypes[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 13)
        {
            DestroyBullet();//replace this with an explosion
        }
        else
        {
            DestroyBullet();
        }
    }
}
