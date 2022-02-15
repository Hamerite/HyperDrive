//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_EBoss_Agnes : S2_BossBaseClass {
    [SerializeField] protected float minxDist, maxxDist, minyDist, maxyDist, waitTime;

    protected Vector3 target;

    protected bool hasTarget, waiting;
    protected float x, y;

    public override void Start() {
        base.Start();
        SetUpBehaviors(0, behavior.lockOn, behavior.disruptor, behavior.chase, behavior.coverage);
        SetUpBehaviors(1, behavior.lockOn, behavior.disruptor, behavior.chase, behavior.coverage);
        SetUpBehaviors(2, behavior.disruptor, behavior.lockOn, behavior.chase, behavior.coverage);
        SetUpBehaviors(3, behavior.disruptor, behavior.lockOn, behavior.chase, behavior.coverage);
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
        //will moveslowly in a random pattern
        target = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, target, stats.GetMinSpeed() * Time.deltaTime);

        if (!waiting && Vector3.Distance(childShip.transform.position, target) < 0.5f) {
            waiting = true;
            Invoke(nameof(Wait), waitTime);
        }
    }

    public void Wait() {
        hasTarget = false;
        waiting = false;
    }

    public override void OnStateChange() {
        base.OnStateChange();
        if (currentHealth == healthState.full) {
            if (currentbehavior == behavior.disruptor) {
                for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(false); }
            }
            else { for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(true); } }
        }
        else if(currentHealth == healthState.threeQuater || currentHealth == healthState.half) {
            if (currentbehavior == behavior.disruptor || currentbehavior == behavior.coverage) {
                for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(false); }
            }
            else { for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(true); } }
        }
        else if (currentHealth == healthState.quater) {
            if (currentbehavior == behavior.chase) {
                for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(true); }
            }
            else { for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(false); } }
        }
    }
}
