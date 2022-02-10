//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
//Modified 10/20/21 (Kyle Ennis)
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SearchService;

public class S2_EnemyBaseClass : MonoBehaviour
{
    [SerializeField] protected S2_EnemyBaseStats stats = null;
    [SerializeField] protected ParticleSystem deathParticles;
    [SerializeField] protected Transform canonPosition;
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
    protected bool spawned = false, canShoot = true, hasCharged = false;
    public bool isMoving = false;
    protected float shotTime = 1.0f, zPos = 25;
    protected int shotMultiple = 0, life, maxLife, shields, firePower;
    public virtual void TopUp()
    {
        life = maxLife;
    }
    public virtual int ReturnLife()
    {
        return life;
    }
    public virtual int GetMaxHealth()
    {
        return maxLife;
    }
    public virtual void Upgrade()
    {
        maxLife++;
        shotTime -= 0.15f;
        TopUp();
    }
    public virtual int GetLife()
    {
        return life;
    }
   
    
    //BoxCast Check
    protected float m_MaxDistance = 50;
    protected bool m_HitDetect;
    [SerializeField] protected BoxCollider m_Collider;
    protected RaycastHit m_Hit;

    private void OnEnable()
    {
        firePower = stats.GetFirePower();
        shields = stats.GetShields();
        maxLife = stats.GetHealth();
        shotTime = stats.GetShotTime();
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

    public virtual void Shoot()
    {
        //if (S2_EnemyBulletPooler.Instance.GetBullets().Count > 0)
        //{
        //    switch (myClass)
        //    {
        //        case ShipClass.speeder:
        //            canShoot = false;
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = canonPosition.position;
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetType(0);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetShotSpeed(20);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);

        //            shotMultiple++;

        //            if (shotMultiple >= 3) Invoke(nameof(EndShot), 0.1f);
        //            else
        //            {
        //                Invoke(nameof(Shoot), 0.1f);
        //            }
        //            break;

        //        case ShipClass.tank:
        //            canShoot = false;

        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = canonPosition.position;
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetType(0);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetShotSpeed(40);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
        //            shotMultiple++;

        //            if (shotMultiple >= 10) Invoke(nameof(EndShot), 1f);
        //            else
        //            {
        //                Invoke(nameof(Shoot), 0.05f);
        //            }
        //            break;

        //        case ShipClass.bomber:
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = canonPosition.position;
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetType(1);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].GetComponent<S2_BulletScript>().SetShotSpeed(5);
        //            S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
        //            Invoke(nameof(Shoot), 2f);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //else
        //    return;
    }


    void FixedUpdate()
    {
        //switch (myClass)
        //{
        //    case ShipClass.speeder:
        //        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        //        if (m_HitDetect)
        //        {
        //            if (m_Hit.collider.gameObject.layer == 13 && canShoot) Shoot();
        //        }
        //        break;
        //    case ShipClass.tank:
        //        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        //        if (m_HitDetect)
        //        {
        //            if (m_Hit.collider.gameObject.layer == 13 && canShoot) Shoot();
        //        }
        //        break;
        //    case ShipClass.bomber:
        //        break;
        //    default:
        //        break;
        //}
      
    }

    public virtual void PlayerDetection()//place in fixed update
    {
        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        if (m_HitDetect)
        {
            if (m_Hit.collider.gameObject.layer == 13 && canShoot) Shoot();
        }
    }
    public virtual void EndShot()
    {
        shotMultiple = 0;
        hasCharged = false;
        canShoot = true;
        return;
    }


    public virtual void TakeDamage()
    {
        life--;
        if (life <= 0)
            Die();
        else
            return;
    }

    public virtual void Die()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        S2_HUDUI.Instance.EnemyKilled(1);
        S2_HUDUI.Instance.SetScore(stats.GetPointsValue());
        S2_EnemySpawnManager.Instance.RemoveFromEnemiesInWave(this);
        Destroy(this.gameObject);
    }


}
