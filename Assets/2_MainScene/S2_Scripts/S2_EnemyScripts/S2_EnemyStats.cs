//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class S2_EnemyStats : MonoBehaviour
{
    [SerializeField] protected S2_EnemyBaseClass stats = null;
    [SerializeField] protected ParticleSystem deathParticles;
    public enum ShipClass { speeder, tank, bomber};
    public ShipClass myClass;
    float checkRadius = 2.5f;
    private Vector3 target;
    public Vector3 GetTargetPosition()
    {
        return target;
    }
    public LayerMask enemyLayer;
    public void SetTarget(Vector3 targetPos)
    {
        target = targetPos;
    }
    bool spawned = false, canShoot = false, hasMoved = false;
    float shotTime = 1.0f, zPos = 10;
    public int shotMultiple = 0, life = 1;
    private void OnEnable()
    {
        //InvokeRepeating(nameof(Shoot), 5, 3);
    }
    Vector3 targetPosition;
    void Update()
    {
        Vector3 p1 = transform.position;
        Collider[] enemyCollider = Physics.OverlapSphere(p1,checkRadius,enemyLayer);
        if (enemyCollider.Length > 0)
        {
            if (!hasMoved)
            {
                Invoke(nameof(ChangeHasMoved), 0.1f);
                hasMoved = true;
                Vector3 direction = (p1 - enemyCollider[0].transform.position).normalized;
                Vector3 targetDirection = (target - p1).normalized;
                targetPosition = direction + targetDirection;
                targetPosition *= 3;
                targetPosition.z = 10;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.GetMaxSpeed() * Time.deltaTime);
        }
        else
        {
            if (!hasMoved)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, target.y, 10), stats.GetMaxSpeed() * Time.deltaTime);
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, target.y, 10), stats.GetMaxSpeed() * Time.deltaTime);
        }
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
                        Invoke(nameof(Shoot), 0.1f);
                    break;
                case ShipClass.tank:
                    //canShoot = true;
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = transform.position;
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
                    //Invoke(nameof(EndShot), 0.1f);
                    break;
                case ShipClass.bomber:
                    //canShoot = true;
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.position = transform.position;
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                    //S2_EnemyBulletPooler.Instance.GetBullets()[0].gameObject.SetActive(true);
                    //Invoke(nameof(Shoot), 2f);
                    break;
                default:
                    break;
            }
        }
        else
            return;
    }

    void ChangeHasMoved()
    {
        hasMoved = !hasMoved;
        //ChooseNearest(transform.position, S2_PointsPlanesCheckIn.Instance.GetUpNext());
    }



    void EndShot()
    {
        shotMultiple = 0;
        return;
    }


    public void TakeDamage()
    {
        life--;
        print("DAMAGE!");
        if (life <= 0)
            Die();
        else
            return;
    }

    public void Die()
    {
        print("Im dead stop!");
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }


}
