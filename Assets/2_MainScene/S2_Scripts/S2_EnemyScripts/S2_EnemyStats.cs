//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SearchService;

public class S2_EnemyStats : MonoBehaviour
{
    [SerializeField] protected S2_EnemyBaseClass stats = null;
    [SerializeField] protected ParticleSystem deathParticles;
    [SerializeField] Transform canonPosition;
    public enum ShipClass { speeder, tank, bomber};
    public ShipClass myClass;
    private Vector3 target, targetPosition;
    
    public Vector3 GetTargetPosition()
    {
        return target;
    }
    
    public void SetTarget(GameObject targetPos)
    {
        target = targetPos.transform.position;
    }

    public LayerMask enemyLayer;
    bool spawned = false, canShoot = true, hasCharged = false;
    public bool isMoving = false;
    float shotTime = 1.0f, zPos = 10, checkRadius = 2.5f;
    int shotMultiple = 0, life = 1;

    //BoxCast Check
    float m_MaxDistance = 50;
    bool m_HitDetect;
    [SerializeField] BoxCollider m_Collider;
    RaycastHit m_Hit;

    private void OnEnable()
    {
        if(myClass == ShipClass.bomber)
            InvokeRepeating(nameof(Shoot), 5, 3);
    }
    void Update()
    {
        //Vector3 p1 = transform.position;
        //Collider[] enemyCollider = Physics.OverlapSphere(p1, checkRadius, enemyLayer);
        //if (enemyCollider.Length > 0)
        //{
        //    isMoving = false;
        //    Vector3 direction = (p1 - enemyCollider[0].transform.position).normalized;
        //    Vector3 targetDirection = (target - p1).normalized;
        //    targetPosition = direction + targetDirection;
        //    targetPosition *= 3;
        //    targetPosition.z = 10;
        //    targetPosition.x = transform.position.x;
        //    transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.GetMaxSpeed() * Time.deltaTime);
        //}
        //if(isMoving)
        //{
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, target.y, 25), stats.GetMaxSpeed() * Time.deltaTime);
        //}
    }

    public void Shoot()
    {
        if (S2_EnemyBulletPooler.Instance.GetBullets().Count > 0)
        {
            switch (myClass)
            {
                case ShipClass.speeder:
                    //canShoot = false;
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = canonPosition.position;
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetType(0);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetShotSpeed(20);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);

                    //shotMultiple++;

                    //if (shotMultiple >= 3) Invoke(nameof(EndShot), 0.1f);
                    //else
                    //{
                    //    Invoke(nameof(Shoot), 0.1f);
                    //}
                    break;

                case ShipClass.tank:
                    //canShoot = false;

                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = canonPosition.position;
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetType(0);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetShotSpeed(40);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
                    //shotMultiple++;

                    //if (shotMultiple >= 10) Invoke(nameof(EndShot), 1f);
                    //else
                    //{
                    //    Invoke(nameof(Shoot), 0.05f);
                    //}
                    break;

                case ShipClass.bomber:
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = canonPosition.position;
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetType(1);
                    S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetShotSpeed(5);
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


    void FixedUpdate()
    {
        switch (myClass)
        {
            case ShipClass.speeder:
                m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
                if (m_HitDetect)
                {
                    if (m_Hit.collider.gameObject.layer == 13 && canShoot) Shoot();
                }
                break;
            case ShipClass.tank:
                m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
                if (m_HitDetect)
                {
                    if (m_Hit.collider.gameObject.layer == 13 && canShoot) Shoot();
                }
                break;
            case ShipClass.bomber:
                break;
            default:
                break;
        }
      
    }

    void EndShot()
    {
        shotMultiple = 0;
        hasCharged = false;
        canShoot = true;
        return;
    }


    public void TakeDamage()
    {
        life--;
        if (life <= 0)
            Die();
        else
            return;
    }

    public void Die()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }


}
