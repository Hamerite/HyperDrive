using UnityEngine;

public class S2_VEBoss_Tanker : S2_BossBaseClass
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

    public override void SetWeaknesses()
    {
        base.SetWeaknesses();
        for(int i =0; i < weakPoints.Length; i++)
        {
            weakPoints[i].SetVulnerablility(true);
        }
    }

    public override void OnStateChange()
    {
        base.OnStateChange();
        if (currentHealth == healthState.half || currentHealth == healthState.quater)
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
    }

}
 