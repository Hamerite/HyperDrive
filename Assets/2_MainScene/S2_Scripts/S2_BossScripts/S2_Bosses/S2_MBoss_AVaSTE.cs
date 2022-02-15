//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_MBoss_AVaSTE : S2_BossBaseClass {
    [SerializeField] protected Transform[] flourishPoints;

    [SerializeField] protected float waitTime;

    protected Vector3 target;

    protected bool waiting, eyeVulnerable, leftVulnerability, rightVulnerability;
    protected bool[] status;
    protected int nextPoint = 0, leftDmg, rightDmg, eyeDmg, maxEyeDmg, ltDmg, rtDmg;

    public override void Start() {
        base.Start();
        SetUpBehaviors(0, behavior.lockOn, behavior.chase, behavior.disruptor, behavior.coverage);
        SetUpBehaviors(1, behavior.lockOn, behavior.chase, behavior.coverage, behavior.disruptor);
        SetUpBehaviors(2, behavior.lockOn, behavior.coverage, behavior.chase, behavior.disruptor);
        SetUpBehaviors(3, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
    }

    public override void Update() { base.Update(); }

    public override void Flourish() {
        base.Flourish();

        target = flourishPoints[nextPoint].position; //will in a clockwise pattern
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, target, stats.GetMinSpeed() * Time.deltaTime);

        if (Vector3.Distance(childShip.transform.position, target) < 0.5f) {
            if (!waiting) {
                waiting = true;
                Invoke(nameof(Wait), waitTime);
            }
        }
    }

    public void Wait() {
        if (nextPoint < flourishPoints.Length - 1) { nextPoint++; }
        else { nextPoint = 0; }

        waiting = false;
    }
    
    #region Vulnerability Switching
    public override void OnStateChange() {
        base.OnStateChange();

        if (currentHealth == healthState.full) { status = new bool[4] { true, true, false, false }; }
        else if(currentHealth == healthState.threeQuater) {
            status = new bool[4] { true, false, true, true };

            if(leftDmg > 8) { status[2] = false; }
            if(rightDmg > 8) { status[3] = false; }
        }
        else if(currentHealth == healthState.half) {
            status = new bool[4] { true, false, false, false };

            if (eyeVulnerable) { status[1] = true; }
        }
        else if(currentHealth == healthState.quater) {
            status = new bool[4] { true, false, false, false };

            if (leftVulnerability) { status[2] = true; }
            if (rightVulnerability) { status[3] = true; }   
        }

        for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(status[i]); }
    }

    public override void TakeDamage(int dmg, bool weak, S2_BossWeakPoint weakPoint) {
        base.TakeDamage(dmg, weak, weakPoint);
        if(!weak) return;  

        if (currentHealth == healthState.threeQuater) {
            if (weakPoint == weakPoints[2]) {
                leftDmg++;
                if (leftDmg > 8) { weakPoints[2].SetVulnerablility(false); }
            }
            if (weakPoint == weakPoints[3]) {
                rightDmg++;
                if (rightDmg > 8) { weakPoints[3].SetVulnerablility(false); }
            }
        }
        else if (currentHealth == healthState.half) {
            if (weakPoint == weakPoints[1]) {
                eyeDmg++;
                if (eyeDmg >= maxEyeDmg) {
                    eyeVulnerable = false;
                    weakPoints[1].SetVulnerablility(false);
                    Invoke(nameof(EyeSwitch), 5);
                }
            }
        }
        else if (currentHealth == healthState.quater) {
            if (weakPoint == weakPoints[2]) {
                ltDmg++;
                if (ltDmg >= 3) {
                    weakPoints[2].SetVulnerablility(false);
                    weakPoints[3].SetVulnerablility(true);
                    ltDmg = 0;
                    rtDmg = 0;
                }
            }
            else if (weakPoint == weakPoints[3]) {
                rtDmg++;
                if (rtDmg >= 3) {
                    weakPoints[2].SetVulnerablility(true);
                    weakPoints[3].SetVulnerablility(false);
                    rtDmg = 0;
                    ltDmg = 0;
                }
            }
        }

    }

    public void EyeSwitch() {
        eyeVulnerable = true;
        weakPoints[1].SetVulnerablility(true);
        maxEyeDmg += 5;
    }
    #endregion

    public override void ResetBoss() {
        base.ResetBoss();
        eyeVulnerable = true;
        rightVulnerability = false;
        leftVulnerability = true;

        nextPoint = 0;
        leftDmg = 0;
        rightDmg = 0;
        maxEyeDmg = 5;
        ltDmg = 0;
        rtDmg = 0;

        status = new bool[4] { true, true, false, false };
        for (int i = 0; i < weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(status[i]); }
    }
}
