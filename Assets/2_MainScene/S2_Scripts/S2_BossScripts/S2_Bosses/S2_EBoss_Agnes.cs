using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EBoss_Agnes : S2_BossBaseClass
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetUpBehaviors(0, behavior.lockOn, behavior.disruptor, behavior.chase, behavior.coverage);
        SetUpBehaviors(1, behavior.lockOn, behavior.disruptor, behavior.chase, behavior.coverage);
        SetUpBehaviors(2, behavior.disruptor, behavior.lockOn, behavior.chase, behavior.coverage);
        SetUpBehaviors(3, behavior.disruptor, behavior.lockOn, behavior.chase, behavior.coverage);
    }

    public override void Update()
    {
        base.Update();
    }

    [SerializeField] float minxDist;
    [SerializeField] float maxxDist;
    [SerializeField] float minyDist;
    [SerializeField] float maxyDist;
    float x;
    float y;
    bool hasTarget;
    Vector3 target;
    [SerializeField] float waitTime;
    bool waiting;

    public override void Flourish()
    {
        base.Flourish();
        if (!hasTarget)
        {
            x = Random.Range(minxDist, maxxDist);
            y = Random.Range(minyDist, maxyDist);
            int randX = Random.Range(-1, 2);
            x *= randX;
            int randY = Random.Range(-1, 2);
            y *= randY;
            //target = new Vector3(x, y, transform.position.z);
            hasTarget = true;
        }
        //will moveslowly in a random pattern
        target = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, target, stats.GetMinSpeed() * Time.deltaTime);

        if (Vector3.Distance(childShip.transform.position, target) < 0.5f)
        {
            //hasTarget = false;
            if (!waiting)
            {
                waiting = true;
                Invoke(nameof(Wait), waitTime);
            }
        }
    }

    public void Wait()
    {
        hasTarget = false;
        waiting = false;
    }

    public override void OnStateChange()
    {
        base.OnStateChange();
        if (currentHealth == healthState.full)
        {
            if (currentbehavior == behavior.disruptor)
            {
                foreach (S2_BossWeakPoint wp in weakPoints)
                {
                    wp.SetVulnerablility(false);
                }
            }
            else
            {
                foreach (S2_BossWeakPoint wp in weakPoints)
                {
                    wp.SetVulnerablility(true);
                }
            }
        }
        else if(currentHealth == healthState.threeQuater || currentHealth == healthState.half)
        {
            if (currentbehavior == behavior.disruptor || currentbehavior == behavior.coverage)
            {
                foreach (S2_BossWeakPoint wp in weakPoints)
                {
                    wp.SetVulnerablility(false);
                }
            }
            else
            {
                foreach (S2_BossWeakPoint wp in weakPoints)
                {
                    wp.SetVulnerablility(true);
                }
            }
        }
        else if (currentHealth == healthState.quater)
        {
            if (currentbehavior == behavior.chase)
            {
                foreach (S2_BossWeakPoint wp in weakPoints)
                {
                    wp.SetVulnerablility(true);
                }
            }
            else
            {
                foreach (S2_BossWeakPoint wp in weakPoints)
                {
                    wp.SetVulnerablility(false);
                }
            }
        }

    }




}
