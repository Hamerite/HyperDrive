//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using System.Collections;
using UnityEngine;

public class S2_HBoss_Strozzi : S2_BossBaseClass {
    [SerializeField] protected S2_Boss_TempShield blastShield;
    [SerializeField] protected S2_Boss_SwitchOrb[] orbs;

    [SerializeField] protected GameObject pathObj;
    [SerializeField] protected Transform centrePoint;
    [SerializeField] protected Transform[] flourishPaths; //multiple spline paths required to make a pattern

    [SerializeField] protected float weakPointOpenTime, rotationSpeed;

    protected Coroutine followFPath;
    protected WaitForEndOfFrame endFrame = new WaitForEndOfFrame();
    protected Vector3 targetFPosition;
    protected Vector3[] flourishPoints = new Vector3[4];

    protected bool cycleOrbs, shieldDestroyed, moveToNextF;
    protected int orbNumber, orbsDestroyed, nextFPath, index;
    protected float tF;

    public override void Start() {
        base.Start();
        SetUpBehaviors(0, behavior.disruptor, behavior.coverage, behavior.chase, behavior.lockOn);
        SetUpBehaviors(1, behavior.disruptor, behavior.coverage, behavior.lockOn, behavior.chase);
        SetUpBehaviors(2, behavior.disruptor, behavior.lockOn, behavior.coverage, behavior.chase);
        SetUpBehaviors(3, behavior.disruptor, behavior.lockOn, behavior.coverage, behavior.chase);
    }

    public override void Update() {
        base.Update();
        if (!cycleOrbs) {
            cycleOrbs = true;
            if (currentHealth == healthState.threeQuater) { orbNumber = 1; }
            else if (currentHealth == healthState.half) { orbNumber = 2; }
            else if (currentHealth == healthState.quater) {
                orbNumber = 3;
                if (!shieldDestroyed) { blastShield.gameObject.SetActive(true); }    
            }

            if (currentHealth != healthState.full) {
                for (int i = 0; i < orbNumber; i++) { orbs[i].gameObject.SetActive(true); }
            }
        }
    }

    public override void OrbDestroyed() {
        base.OrbDestroyed();
        orbsDestroyed++;
        if(orbsDestroyed >= orbNumber) {
            if (currentHealth != healthState.quater) {
                weakPoints[1].SetVulnerablility(true);
                Invoke(nameof(ResetOrbs), weakPointOpenTime);
            }
            else { blastShield.gameObject.SetActive(true); }
        }
    }

    public override void DestroyShield() {
        base.DestroyShield();
        shieldDestroyed = true;
        weakPoints[1].SetVulnerablility(true);
        Invoke(nameof(ResetOrbs), weakPointOpenTime);
    }

    public void ResetOrbs() {
        orbsDestroyed = 0;
        weakPoints[1].SetVulnerablility(false);
        cycleOrbs = false;
    }

    public override void Flourish() {
        base.Flourish();
       
        pathObj.transform.Rotate(0,0, rotationSpeed * Time.deltaTime);
        if(moveToNextF) { followFPath = StartCoroutine(FollowFlourishPath(nextFPath)); }        
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

        tF= 0;
        nextFPath++;
        if (nextFPath > flourishPaths.Length - 1) { nextFPath = 0; }
        moveToNextF = true;
    }

    public override void OnStateChange() {
        base.OnStateChange();
        index++;
       
        if(currentHealth == healthState.full && currentbehavior == behavior.disruptor && index >= 5) { weakPoints[1].SetVulnerablility(true); }      
    }   

    public override void TakeDamage(int dmg, bool weak, S2_BossWeakPoint weakPoint) { base.TakeDamage(dmg, weak, weakPoint); }

    public override void ResetBoss() {
        base.ResetBoss();
        cycleOrbs = false;
        shieldDestroyed = false;

        index = 0;
        nextFPath = 0;
        tF = 0;
        moveToNextF = true;
    }
}
