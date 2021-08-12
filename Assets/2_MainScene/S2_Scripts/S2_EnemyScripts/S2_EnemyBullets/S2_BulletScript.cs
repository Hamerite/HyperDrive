using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BulletScript : MonoBehaviour
{
    [SerializeField] float shotSpeed = 5, shotDamage = 1;
    [SerializeField] bool fired;
    Vector3 startPos, endPos;
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
    }
    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            //if ((transform.position.z - endPos.z) > 0.5f)
            //    transform.position = Vector3.Lerp(transform.position, endPos, shotSpeed * Time.deltaTime);
            //else
            //    transform.position = Vector3.Lerp(transform.position, new Vector3(endPos.x, endPos.y, endPos.z - 30), shotSpeed * Time.deltaTime);
             transform.position += transform.forward * shotSpeed * Time.deltaTime;
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
