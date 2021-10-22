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
    [SerializeField]
    float shields;
    [SerializeField]
    float speed;

    [SerializeField]
    public GameObject childShip;
    
    
    //for boss behaviors
    public enum behavior { enter, coverage, lockOn, chase, disruptor}
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

    //GameObject player;

    //for boss movement patterns, using splines
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
        transform.position = startLocation.position;
        currentbehavior = behavior.enter;

        //following paths
        nextPath = 0;
        t = 0;
        moveToNext = false;
        S2_BossManager.Instance.UpdateText(health, shields);
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
        }

        if (moveToNext)
        {
            followPath = StartCoroutine(FollowPath(nextPath));
        }

        //timer to change which behavior is active
        if (currentbehavior != behavior.enter)
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
        transform.position = Vector3.MoveTowards(transform.position, paths[0].GetChild(0).position, speed * 1.5f * Time.deltaTime);
        if(Vector3.Distance(transform.position, paths[0].GetChild(0).position) < 0.5f)
        {
            currentbehavior = behaviorOrder[orderNo][cycleNo];
            moveToNext = true;
        }
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

            transform.position = targetPosition;
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
        if (currentbehavior == behavior.chase) //need an intermediate state to move towards the starting point of the path
        {
            InvokeRepeating(nameof(RapidFire), 2, 0.15f); //each boss will have it's own rapid fire rate            
        }
        else
        {
            moveToNext = true;
            CancelInvoke(nameof(RapidFire));
            //followPath = StartCoroutine(FollowPath(nextPath));
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
                    x = Random.Range(-12, 1);
                    y = Random.Range(1, 5);
                }
                else if(i == 1)
                {
                    x = Random.Range(1, 12);
                    y = Random.Range(1, 5);
                }
                else if (i == 2)
                {
                    x = Random.Range(-12, 1);
                    y = Random.Range(-5, -1);
                }
                else if (i == 3)
                {
                    x = Random.Range(1, 12);
                    y = Random.Range(-5, -1);
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
        }
        else { health -= dmg; }

        if (health <= 0.25f * stats.GetAttributes()[3])
        {
            if (currentHealth != healthState.quater)
            {
                orderNo = 3;
                cycleNo = 0;
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
                currentHealth = healthState.threeQuater;
            }
            //currentbehavior = behaviorOrder[orderNo][cycleNo];
            //OnStateChange();
            //timer = 0;
        } 

        if(health <= 0)
        {
            S2_BossManager.Instance.EndBoss();
        }

        //updates the UI. will change this to a bar once we get assets in
        S2_BossManager.Instance.UpdateText(health, shields);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(RapidFire));
        CancelInvoke(nameof(HomingShots));
        CancelInvoke(nameof(CoverageShot));
    }

    public void AddScore()
    {
        //S2_HUDUI.Instance.EnemyKilled(stats.GetPointsValue()); commeented out until merge
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
