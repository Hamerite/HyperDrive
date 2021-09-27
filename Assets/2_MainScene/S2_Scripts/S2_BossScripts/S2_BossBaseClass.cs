//create 21/09/21 by AT
//handles the basic boss behaviors
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossBaseClass : MonoBehaviour
{
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
    public enum behavior { coverage, lockOn, chase, disruptor}
    public behavior currentbehavior;
    //to be used to set the behavior
    behavior[][] behaviorOrder = new behavior[4][];
    int orderNo;
    int cycleNo;

    [SerializeField]
    float maxTimePerCycle;
    float timer;



    //differnet types of projectiles (dont actually need this when using the pool)
    //[SerializeField]
    //S2_BossCoverageBase coverageObject;
    //[SerializeField]
    //S2_BossHomingBase homingObject;
    //[SerializeField]
    //S2_BossChaserBase chasingObject;
    //[SerializeField]
    //S2_BossDisruptorBase disruptorObject;

    //
    [SerializeField]
    S2_BossWeakPoint[] weakPoints;
    [SerializeField]
    Transform launchPoint;

    GameObject player;

    //for boss movement patterns, using splines
    [SerializeField]
    Transform[] paths; //multiple spline paths required to make a pattern
    private int nextPath;
    float t;
    bool moveToNext;
    Vector3[] points = new Vector3[4];
    Vector3 targetPosition;
    Coroutine followPath;


    // Start is called before the first frame update
    public virtual void Start()
    {
        player = S2_PlayerController.Instance.gameObject;
        orderNo = 0;
        cycleNo = 0;
        health = stats.GetAttributes()[2];
        shields = stats.GetAttributes()[3];
        speed = stats.GetMaxSpeed();
        for(int i = 0; i < behaviorOrder.Length; i++)
        {
            behaviorOrder[i] = new behavior[4];
        }
        //each boss will have set it's own behaviors using the setup behavior
        currentHealth = healthState.full;

        //following paths
        nextPath = 0;
        t = 0;
        moveToNext = true;
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


    // Update is called once per frame
    public virtual void Update()
    {
        Movement();
        Flourish();
    }

    public virtual void Movement() //for the general screenwide pattern
    {
        switch(currentbehavior)
        {
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
    }

    public virtual void Flourish() //for the smaller area movement, each boss will have a different action
    {

    }


    public virtual void Coverage() //coverage behavior will have the boss follow its normal pattern
    {
        //if (moveToNext)
        //{
        //    followPath = StartCoroutine(FollowPath(nextPath));
        //}

    }
    public virtual void LockOn() //lock on behavior will have the boss follow its normal pattern
    {
        //if (moveToNext)
        //{
        //    followPath = StartCoroutine(FollowPath(nextPath));
        //}
    }
    public virtual void Chase()//chase behavior will have the boss follow the player around
    {
        //Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
    
    
    }
    public virtual void Disruptor() //disruptor behavior will have the boss follow its normal pattern
    {
        //if (moveToNext)
        //{
        //    followPath = StartCoroutine(FollowPath(nextPath));
        //}
    }

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
            yield return new WaitForEndOfFrame();
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

    public void OnStateChange()
    {
        StopCoroutine(followPath);
        if (currentbehavior == behavior.chase) //need an intermediate state to move towards the starting point of the path
        {
            InvokeRepeating(nameof(RapidFire), 0, 0.15f); //each boss will have it's own rapid fire rate            
        }
        else
        {
            moveToNext = true;
            CancelInvoke(nameof(RapidFire));
            //followPath = StartCoroutine(FollowPath(nextPath));
        }

        if(currentbehavior == behavior.lockOn)
        {
            InvokeRepeating(nameof(HomingShots), 0, 1);//change this back to 
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

    //used during chase phase
    public void RapidFire()
    {
        GameObject bullet = S2_BossBulletPooler.Instance.GetChaseBullet();
        bullet.transform.position = launchPoint.position;
        bullet.transform.LookAt(player.transform.position);
        bullet.SetActive(true);
    }

    public void HomingShots()
    {
        GameObject bullet = S2_BossBulletPooler.Instance.GetLockOnBullet();
        bullet.transform.position = launchPoint.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);
    }

    public void ActivateDisruptors()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject bullet = S2_BossBulletPooler.Instance.GetDisruptorBullet();
            int x = Random.Range(-8, 9);
            int y = Random.Range(-4, 5);
            bullet.transform.position = new Vector3(x, y, player.transform.position.z);
            bullet.SetActive(true);
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

    public enum healthState { full, threeQuater, half, quater}
    public healthState currentHealth;

    public virtual void TakeDamage(int dmg)
    {
        Mathf.Round(shields);
        Mathf.Round(health);
        if(shields > 0)
        {
            shields -= dmg;
            if(shields < 0)
            {
                health -= Mathf.Abs(shields);
                shields = 0;
            }
        }
        else { health -= dmg; }

        if (health <= 0.25f * stats.GetAttributes()[3] && currentHealth != healthState.quater)
        {
            orderNo = 3;
            cycleNo = 0;
            currentHealth = healthState.quater;
            currentbehavior = behaviorOrder[orderNo][cycleNo];
            OnStateChange();
            timer = 0;
        }
        else if (health <= 0.5f * stats.GetAttributes()[3] && currentHealth != healthState.half)
        {
            orderNo = 2;
            cycleNo = 0;
            currentHealth = healthState.half;
            currentbehavior = behaviorOrder[orderNo][cycleNo];
            OnStateChange();
            timer = 0;
        }
        else if (health <= 0.75f * stats.GetAttributes()[3] && currentHealth != healthState.threeQuater)
        {
            orderNo = 1;
            cycleNo = 0;
            currentHealth = healthState.threeQuater;
            currentbehavior = behaviorOrder[orderNo][cycleNo];
            OnStateChange();
            timer = 0;
        }       
    }

   
}
