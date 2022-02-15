//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using System.Collections;
using UnityEngine;

public class S2_VHBoss_Amour : S2_BossBaseClass {
    [SerializeField] protected S2_Boss_TempShield[] tempShields;

    [SerializeField] protected GameObject pathObj;
    [SerializeField] protected Transform centrePoint;
    [SerializeField] protected Transform[] flourishPaths; //multiple spline paths required to make a pattern

    [SerializeField] protected float rotationSpeed;

    protected Coroutine followFPath;
    protected WaitForEndOfFrame endFrame = new WaitForEndOfFrame();
    protected Vector3 targetFPosition;
    protected Vector3[] flourishPoints = new Vector3[4];

    protected bool started, moveToNextF;
    protected int activeWeakPoint, dmgCounter, nextFPath;
    protected float openTime, tF;

    public override void Start() {
        base.Start();
        SetUpBehaviors(0, behavior.lockOn, behavior.coverage, behavior.chase, behavior.disruptor);
        SetUpBehaviors(1, behavior.lockOn, behavior.coverage, behavior.disruptor, behavior.chase);
        SetUpBehaviors(2, behavior.disruptor, behavior.coverage, behavior.lockOn, behavior.chase);
        SetUpBehaviors(3, behavior.coverage, behavior.disruptor, behavior.lockOn, behavior.chase);
    }

    public override void Update() { base.Update(); }   

    public override void OnStateChange() {
        base.OnStateChange();
        if(currentbehavior != behavior.enter && !started){
            CycleWeakPoints();
            started = true;
        }
    }

    public void CycleWeakPoints() {
        if(currentHealth == healthState.full || currentHealth == healthState.threeQuater) { activeWeakPoint = Random.Range(1, weakPoints.Length); } //starts at 1 to prevent only having body available to shoot
        else if(currentHealth == healthState.half) { activeWeakPoint = Random.Range(1, weakPoints.Length - 1); }   
        else if (currentHealth == healthState.quater) { activeWeakPoint = Random.Range(1, weakPoints.Length - 2); }

        weakPoints[activeWeakPoint].SetVulnerablility(true);
        if (currentHealth == healthState.half || currentHealth == healthState.quater) { tempShields[activeWeakPoint - 1].gameObject.SetActive(true); }

        if (currentHealth == healthState.full) { openTime = 7; }
        else { openTime = 5; }

        Invoke(nameof(TurnOffWeakPoint), openTime);
    }

    public void TurnOffWeakPoint() {
        weakPoints[activeWeakPoint].SetVulnerablility(false);
        if(tempShields[activeWeakPoint - 1].gameObject.activeInHierarchy) { tempShields[activeWeakPoint - 1].gameObject.SetActive(false); }

        Invoke(nameof(CycleWeakPoints), 1);
    }

    #region flourish
    public override void Flourish() {
        base.Flourish();

        pathObj.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        if (moveToNextF) { followFPath = StartCoroutine(FollowFlourishPath(nextFPath)); }
    }

    
    IEnumerator FollowFlourishPath(int pathNum) {
        moveToNextF = false;

        while (tF < 1) {
            flourishPoints[0] = flourishPaths[pathNum].GetChild(0).position;
            flourishPoints[1] = flourishPaths[pathNum].GetChild(1).position;
            flourishPoints[2] = flourishPaths[pathNum].GetChild(2).position;
            flourishPoints[3] = flourishPaths[pathNum].GetChild(3).position;

            tF += Time.deltaTime * stats.GetMinSpeed() / 10;
            targetFPosition = Mathf.Pow(1 - tF, 3) * flourishPoints[0] + 3 * Mathf.Pow(1 - tF, 2) * tF * flourishPoints[1] + 3 * (1 - tF) * Mathf.Pow(tF, 2) * flourishPoints[2] + Mathf.Pow(tF, 3) * flourishPoints[3]; //bezier curve

            childShip.transform.position = targetFPosition;
            yield return endFrame;
        }

        tF = 0;
        nextFPath++;
        if (nextFPath > flourishPaths.Length - 1) { nextFPath = 0; }
        moveToNextF = true;
    }
    #endregion

    public override void TakeDamage(int dmg, bool weak, S2_BossWeakPoint weakPoint) {
        base.TakeDamage(dmg, weak, weakPoint);
        if(weakPoint == weakPoints[activeWeakPoint]) {
            dmgCounter++;
            if(dmgCounter >= 5) {
                dmgCounter = 0;
                weakPoint.SetVulnerablility(false);
                CancelInvoke(nameof(CycleWeakPoints));
                Invoke(nameof(CycleWeakPoints), 1);
            }
        }
    }

    public override void ResetBoss() {
        base.ResetBoss();
        started = false;

        nextFPath = 0; //for flourish paths
        tF = 0;
        moveToNextF = true;

        dmgCounter = 0;
        for(int i = 0; i < tempShields.Length; i++) { tempShields[i].gameObject.SetActive(false); }
        for(int i = 1; i<weakPoints.Length; i++) { weakPoints[i].SetVulnerablility(false); }
        weakPoints[0].SetVulnerablility(true);
    }
}
