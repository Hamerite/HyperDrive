//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_VEBoss_Tanker : S2_BossBaseClass {
    [SerializeField] protected float minxDist, maxxDist, minyDist, maxyDist, waitTime;

    protected Vector3 target;

    protected bool hasTarget, waiting;
    protected float x, y;

    public override void Start() {
        base.Start();
        SetUpBehaviors(0, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
        SetUpBehaviors(1, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
        SetUpBehaviors(2, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
        SetUpBehaviors(3, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);        
    }

    public override void Update() { base.Update(); }

    public override void Flourish() {
        base.Flourish();
        if (!hasTarget) {
            x = Random.Range(minxDist, maxxDist);
            y = Random.Range(minyDist, maxyDist);
            int randX = Random.Range(-1, 2);
            int randY = Random.Range(-1, 2);
            x *= randX;
            y *= randY;
            hasTarget = true;
        }
        
        target = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z); //will moveslowly in a random pattern
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, target, stats.GetMinSpeed() * Time.deltaTime);

        if (Vector3.Distance(childShip.transform.position, target) < 0.5f) {
            if (!waiting) {
                waiting = true;
                Invoke(nameof(Wait), waitTime);
            }
        }
    }

    public void Wait() {
        hasTarget = false;
        waiting = false;
    }

    public override void SetWeaknesses() {
        base.SetWeaknesses();
        for(int i =0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(true); }
    }

    public override void OnStateChange() {
        base.OnStateChange();
        if (currentHealth == healthState.half || currentHealth == healthState.quater) {
            if (currentbehavior == behavior.disruptor)  {
                for (int i = 0; i < weakPoints.Length - 1; i++) { weakPoints[i].SetVulnerablility(false); }
            }
            else { for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(false); } }
        }
    }
}
 