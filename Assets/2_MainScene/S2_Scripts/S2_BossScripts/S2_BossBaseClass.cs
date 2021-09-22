using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossBaseClass : MonoBehaviour
{
    [SerializeField]
    protected S2_BossStats stats = null;
    [SerializeField]
    float health;
    [SerializeField]
    float shields;

    float speed;
 
    public enum behavior { coverage, lockOn, chase, disruptor}
    public behavior currentbehavior;
    //to be used to set the behavior
    behavior[][] behaviorOrder = new behavior[4][];
    int orderNo;
    int cycleNo;

    float maxTimePerCycle;
    float timer;

    [SerializeField]
    S2_BossCoverageBase coverageObject;
    [SerializeField]
    S2_BossHomingBase homingObject;
    [SerializeField]
    S2_BossChaserBase chasingObject;
    [SerializeField]
    S2_BossDisruptorBase disruptorObject;

    [SerializeField]
    S2_BossWeakPoint[] weakPoints;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = S2_PlayerController.Instance.gameObject;
        orderNo = 0;
        cycleNo = 0;
        health = stats.GetAttributes()[1];
        shields = stats.GetAttributes()[2];
        speed = stats.GetMaxSpeed();
        for(int i = 0; i < behaviorOrder.Length; i++)
        {
            behaviorOrder[i] = new behavior[4];
        }
        //each boss will have set it's own behaviors using the setup behavior
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
    void Update()
    {
        
    }

    public virtual void Movement()
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

        //timer to change which behavior is active
        timer += Time.fixedDeltaTime;
        if (timer >= maxTimePerCycle)
        {
            if (cycleNo < 3)
            {
                cycleNo += 1;
            }
            else
                cycleNo = 0;

            currentbehavior = behaviorOrder[orderNo][cycleNo];
            if (currentbehavior == behavior.chase)
            {
                InvokeRepeating(nameof(RapidFire), 0, 1); //will each boss have it's own rapid fire rate
            }
            timer = 0;
        }
    }

    public virtual void Coverage()
    { }
    public virtual void LockOn()
    { }
    public virtual void Chase()
    {
        Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed);
    
    
    }
    public virtual void Disruptor()
    { }

    public virtual void CycleThrough()
    {
        if (cycleNo < 3)
        {
            cycleNo += 1;
        }
        else
            cycleNo = 0;

        currentbehavior = behaviorOrder[orderNo][cycleNo];
        if (currentbehavior == behavior.chase)
        {
            InvokeRepeating(nameof(RapidFire), 0, 1); //will each boss have it's own rapid fire rate
        }
        timer = 0;
    }

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

        if (health <= 0.25f * stats.GetAttributes()[2])
        {
            orderNo = 3;
            cycleNo = 0;
        }
        else if (health <= 0.5f * stats.GetAttributes()[2])
        {
            orderNo = 2;
            cycleNo = 0;
        }
        else if (health <= 0.75f * stats.GetAttributes()[2])
        {
            orderNo = 1;
            cycleNo = 0;
        }

        currentbehavior = behaviorOrder[orderNo][cycleNo];
        if(currentbehavior == behavior.chase)
        {
            InvokeRepeating(nameof(RapidFire), 0, 1); //will each boss have it's own rapid fire rate
        }
        timer = 0;
    }

    public void RapidFire()
    {
        //lauch projectiles here
    }

}
