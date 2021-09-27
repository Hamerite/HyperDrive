using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_TestBoss : S2_BossBaseClass
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetUpBehaviors(0, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
        SetUpBehaviors(1, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
        SetUpBehaviors(2, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
        SetUpBehaviors(3, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
    }

    public override void Update()
    {
        base.Update();
    }

    [SerializeField]float xDist;
    [SerializeField]float yDist;
    float x;
    float y;
    bool hasTarget;
    Vector3 target;
    [SerializeField] float waitTime;
    bool waiting;
    
    public override void Flourish()
    {
        base.Flourish();
        if(!hasTarget)
        {
            x = Random.Range(-xDist, xDist);
            y = Random.Range(-yDist, yDist);
            //target = new Vector3(x, y, transform.position.z);
            hasTarget = true;
        }
        //will moveslowly in a random pattern
        target = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, target, stats.GetMinSpeed() * Time.deltaTime);

        if(Vector3.Distance(childShip.transform.position, target) < 0.5f)
        {
            //hasTarget = false;
            if(!waiting)
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
    
}
