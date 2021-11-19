//create 21/09/21 (Alek Tepylo)
//handles the basic boss behaviors
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossBaseClass : MonoBehaviour
{
    #region variables
    //boss properties
    [SerializeField]
    protected S2_BossStats stats = null;
    [SerializeField]
    float health;
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return stats.GetAttributes()[3]; }
    [SerializeField]
    float shields;
    public float GetShields() { return shields; }
    public float GetMaxShields() { return stats.GetAttributes()[2]; }
    [SerializeField]
    float speed;

    [SerializeField]
    public GameObject childShip;

    //for boss behaviors
    public enum behavior { enter, coverage, lockOn, chase, disruptor, exiting }
    public behavior currentbehavior;
    public enum healthState { full, threeQuater, half, quater }
    public healthState currentHealth;

    //to be used to set the behavior
    behavior[][] behaviorOrder = new behavior[4][];
    int orderNo;
    int cycleNo;

    [SerializeField]
    float maxTimePerCycle;
    float timer;

    [SerializeField]
    int disruptorNumber = 5;

    [SerializeField]
    protected S2_BossWeakPoint[] weakPoints;
    [SerializeField]
    Transform launchPoint;
    [SerializeField] Transform startLocation;

    //for boss animations
    [SerializeField] private GameObject[] destroyables;
    GameObject destroyablePiece;
    int destroyCount = 0;
    [SerializeField] ParticleSystem destroyParticles;
    [SerializeField] GameObject shieldEffect;
    [SerializeField] ParticleSystem[] deathParticles;
    private int explosionCount;
    private Vector3 endPosition;

    //GameObject player;

    //for boss movement patterns (paths), using splines
    [SerializeField]
    Transform[] paths; //multiple spline paths required to make a pattern
    private int nextPath;
    float t;
    bool moveToNext;
    Vector3[] points = new Vector3[4];
    Vector3 targetPosition;
    Coroutine followPath;
    #endregion

    #region Setup
    public virtual void Start()
    {        
        //each boss will have set it's own behaviors using the setup behavior
        for(int i = 0; i < behaviorOrder.Length; i++)
        {
            behaviorOrder[i] = new behavior[4];
        }
        ResetBoss(); 
    }

    public virtual void ResetBoss()
    {
        orderNo = 0;
        cycleNo = 0;
        health = stats.GetAttributes()[3];
        shields = stats.GetAttributes()[2];
        speed = stats.GetMaxSpeed();
        currentHealth = healthState.full;
        childShip.transform.position = startLocation.position;
        currentbehavior = behavior.enter;
        Invoke(nameof(SetWeakPoints), 0.1f);
        for(int i=0; i<destroyables.Length; i++)
        {
            destroyables[i].SetActive(true);
        }
        explosionCount = 0;
        for(int i = 0; i < deathParticles.Length; i++)
        {
            deathParticles[i].gameObject.SetActive(false);
        }

        //S2_BossManager.Instance.UpdateText(health, shields);
        //following paths
        nextPath = 0;
        t = 0;
        moveToNext = false;
    }

    public void SetWeakPoints()
    {
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].SetVulnerablility(false);
        }
    }

    public virtual void SetUpBehaviors(int x, behavior b1, behavior b2, behavior b3, behavior b4)
    {
        behaviorOrder[x][0] = b1;
        behaviorOrder[x][1] = b2;
        behaviorOrder[x][2] = b3;
        behaviorOrder[x][3] = b4;
    }
    /* example of set up
     * setupbehavior(i, enterbehavor orders you want here);
     */
    #endregion

    #region behaviors
    public virtual void Update()
    {
        Movement();        
    }

    public virtual void Movement() //for the general screenwide pattern
    {
        switch(currentbehavior)
        {
            case behavior.enter:
                Entre();
                break;
            case behavior.coverage:
                Coverage();
                break;
            case behavior.lockOn:
                LockOn();
                break;
            case behavior.chase:
                Chase();
                break;
            case behavior.disruptor:
                Disruptor();
                break;
            case behavior.exiting:
                Exiting();
                break;
        }

        if (moveToNext)
        {
            followPath = StartCoroutine(FollowPath(nextPath));
        }

        //timer to change which behavior is active
        if (currentbehavior != behavior.enter && currentbehavior != behavior.exiting)
        {
            timer += Time.deltaTime;
            if (timer >= maxTimePerCycle)
            {
                if (cycleNo < 3)
                {
                    cycleNo += 1;
                }
                else
                    cycleNo = 0;

                currentbehavior = behaviorOrder[orderNo][cycleNo];
                OnStateChange();
                timer = 0;
            }

            Flourish();
        }
    }

    public virtual void Flourish() //for the smaller area movement, each boss will have a different action
    {
    }

    public virtual void Entre()
    {
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, paths[0].GetChild(0).position, speed * 1.5f * Time.deltaTime);
        if(Vector3.Distance(childShip.transform.position, paths[0].GetChild(0).position) < 0.5f)
        {
            currentbehavior = behaviorOrder[orderNo][cycleNo];
            moveToNext = true;
            SetWeaknesses();
        }
    }

    public virtual void SetWeaknesses()
    {
    }

    public virtual void Coverage() 
    {
    }
    public virtual void LockOn() 
    {
    }
    public virtual void Chase()
    {       
    }
    public virtual void Disruptor() 
    {        
    }

    public virtual void Exiting()
    {        
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, endPosition, speed * 1.5f * Time.deltaTime);
    }    

    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    //ienumerator used to follow the path
    IEnumerator FollowPath(int pathNum)
    {
        if (moveToNext)
        {
            points[0] = paths[pathNum].GetChild(0).position;
            points[1] = paths[pathNum].GetChild(1).position;
            points[2] = paths[pathNum].GetChild(2).position;
            points[3] = paths[pathNum].GetChild(3).position;
        }
        moveToNext = false;

        while (t < 1)
        {
            t += Time.deltaTime * speed/10;
            //bezier curve
            targetPosition = Mathf.Pow(1 - t, 3) * points[0] + 3 * Mathf.Pow(1 - t, 2) * t * points[1] + 3 * (1 - t) * Mathf.Pow(t, 2) * points[2] + Mathf.Pow(t, 3) * points[3];

            childShip.transform.position = targetPosition;
            yield return endOfFrame;
        }

        t = 0;
        nextPath++;
        if (nextPath > paths.Length - 1)
            nextPath = 0;
              
        moveToNext = true;
    }

    //used to go to next cycle not based on a timer
    public virtual void CycleThrough()
    {
        if (cycleNo < 3)
        {
            cycleNo += 1;
        }
        else
            cycleNo = 0;

        currentbehavior = behaviorOrder[orderNo][cycleNo];
        OnStateChange();
        timer = 0;
    }

    public virtual void OnStateChange()
    {
        StopCoroutine(followPath);
        if (currentbehavior == behavior.chase) 
        {
            InvokeRepeating(nameof(RapidFire), 2, 0.15f); //each boss will have it's own rapid fire rate
        }
        else
        {
            moveToNext = true;
            CancelInvoke(nameof(RapidFire));
        }

        if(currentbehavior == behavior.lockOn)
        {
            InvokeRepeating(nameof(HomingShots), 0, 1);//change this back to each boss
        }
        else
        {
            CancelInvoke(nameof(HomingShots));
        }

        if(currentbehavior == behavior.disruptor)
        {
            Invoke(nameof(ActivateDisruptors), 0);
        }

        if(currentbehavior == behavior.coverage)
        {
            InvokeRepeating(nameof(CoverageShot), 0, 2.5f);
        }
        else
        {
            CancelInvoke(nameof(CoverageShot));
        }

        followPath = StartCoroutine(FollowPath(nextPath));        
    }
    #endregion

    #region weapon fire
    public void RapidFire()
    {
        GameObject bullet = S2_BossBulletPooler.Instance.GetChaseBullet();
        if (bullet != null)
        {
            bullet.transform.position = launchPoint.position;
            bullet.transform.LookAt(S2_PlayerController.Instance.gameObject.transform.position);
            bullet.SetActive(true);
        }       
    }

    public void HomingShots()
    {
        GameObject bullet = S2_BossBulletPooler.Instance.GetLockOnBullet();
        if (bullet != null)
        {
            bullet.transform.position = launchPoint.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }
    }

    public void ActivateDisruptors()
    {
        for(int i = 0; i < disruptorNumber; i++)
        {
            GameObject bullet = S2_BossBulletPooler.Instance.GetDisruptorBullet();
            if (bullet != null)
            {
                int x = 0;
                int y = 0;
                if(i == 0)
                {
                    x = Random.Range(-15, -3);
                    y = Random.Range(2, 7);
                }
                else if(i == 1)
                {
                    x = Random.Range(3, 15);
                    y = Random.Range(2, 7);
                }
                else if (i == 2)
                {
                    x = Random.Range(-15, -3);
                    y = Random.Range(-7, -2);
                }
                else if (i == 3)
                {
                    x = Random.Range(3, 15);
                    y = Random.Range(-7, -2);
                }
                else if (i == 4)
                {
                    x = Random.Range(-3, 3);
                    y = Random.Range(-2, 2);
                }
                bullet.transform.position = new Vector3(x, y, S2_PlayerController.Instance.gameObject.transform.position.z);
                bullet.SetActive(true);
            }
        }
    }

    public void CoverageShot()
    {
        GameObject bullet = S2_BossBulletPooler.Instance.GetCoverageBullet();
        if (bullet != null)
        {
            bullet.transform.position = new Vector3(0, 0, transform.position.z);
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }        
    }
    #endregion

    #region damage and destroying
    public virtual void TakeDamage(int dmg, bool weak, S2_BossWeakPoint weakPoint)
    {
        Mathf.Round(shields);
        Mathf.Round(health);
        if(!weak && shields > 0)
        {
            shields -= dmg;
            if(shields < 0)
            {
                health -= Mathf.Abs(shields);
                shields = 0;
            }
            ShowSheildieldDamage();
        }
        else { health -= dmg; ShowHealthDamage(); }        

        if (health <= 0.25f * stats.GetAttributes()[3])
        {
            if (currentHealth != healthState.quater)
            {
                orderNo = 3;
                cycleNo = 0;
                RemovePieces();
                currentHealth = healthState.quater;
            }
            //currentbehavior = behaviorOrder[orderNo][cycleNo];
            //OnStateChange();
            //timer = 0;
        }
        else if (health <= 0.5f * stats.GetAttributes()[3])
        {
            if (currentHealth != healthState.half)
            {
                orderNo = 2;
                cycleNo = 0;
                RemovePieces();
                currentHealth = healthState.half;
            }
            //currentbehavior = behaviorOrder[orderNo][cycleNo];
            //OnStateChange();
            //timer = 0;
        }
        else if (health <= 0.75f * stats.GetAttributes()[3])
        {
            if (currentHealth != healthState.threeQuater)
            {
                orderNo = 1;
                cycleNo = 0;
                RemovePieces();
                currentHealth = healthState.threeQuater;
            }
            //currentbehavior = behaviorOrder[orderNo][cycleNo];
            //OnStateChange();
            //timer = 0;
        } 

        if(health <= 0)
        {
            DestroyBoss();
        }

        //updates the UI
        S2_BossManager.Instance.UpdateHealthBars(health, shields);
    }

    public virtual void DestroyBoss()
    {
        moveToNext = false;
        StopCoroutine(followPath);
        currentbehavior = behavior.exiting;
        endPosition = new Vector3(childShip.transform.position.x - 5, childShip.transform.position.y - 15, childShip.transform.position.z);
        for(int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].SetVulnerablility(false);
        }
        Invoke(nameof(EndBoss), 3);
        Invoke(nameof(BossExplosion), 0.5f);
    }

    public virtual void EndBoss()
    {
        S2_BossManager.Instance.EndBoss();
    }

    public virtual void BossExplosion()
    {
        if(explosionCount >= deathParticles.Length) { return; }

        deathParticles[explosionCount].gameObject.SetActive(true);
        int randX = Random.Range(-5, 5);
        int randY = Random.Range(-5, 5);
        int randZ = Random.Range(-5, 5);
        deathParticles[explosionCount].transform.position = new Vector3(childShip.transform.position.x + randX, childShip.transform.position.y + randY, childShip.transform.position.z + randZ);
        
        explosionCount++;
        Invoke(nameof(BossExplosion), 0.5f);
    }

    public void ShowHealthDamage()
    {
        if(weakPoints[0].GetMesh().material.color != weakPoints[0].GetColor()) { return; }

        for(int i = 0; i<weakPoints.Length; i++)
        {
            weakPoints[i].GetMesh().material.color = Color.red;
        }
        Invoke(nameof(ResetColours), 0.2f);
    }

    public void ShowSheildieldDamage()
    {
        if (weakPoints[0].GetMesh().material.color != weakPoints[0].GetColor()) { return; }

        shieldEffect.SetActive(true);
        for (int i = 0; i < weakPoints.Length; i++)
        {
            weakPoints[i].GetMesh().material.color = Color.blue;
        }
        Invoke(nameof(ResetColours), 0.2f);
    }

    public void ResetColours()
    {
        for (int i = 0; i < weakPoints.Length; i++)
        {           
            weakPoints[i].GetMesh().material.color = weakPoints[i].GetColor();
            shieldEffect.SetActive(false);
        }
    }   

    public void RemovePieces()
    {
        if (destroyables[destroyCount])
        {
            destroyablePiece = destroyables[destroyCount];
            //play fx/anims here
            destroyParticles.gameObject.SetActive(true);
            destroyParticles.transform.position = destroyablePiece.transform.position;
            Invoke(nameof(DestroyPiece), 1f);
            destroyCount++;
        }
    }

    public void DestroyPiece()
    {
        destroyablePiece.SetActive(false);
        destroyParticles.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(RapidFire));
        CancelInvoke(nameof(HomingShots));
        CancelInvoke(nameof(CoverageShot));
    }

    public void AddScore()
    {
        S2_HUDUI.Instance.EnemyKilled(stats.GetPointValue());
    }
    #endregion

    #region Temp Shields
    public virtual void DestroyShield()
    {

    }

    #endregion

    #region Extent Orbs
    public virtual void OrbDestroyed()
    {

    }


    #endregion
}
