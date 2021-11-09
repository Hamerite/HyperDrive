using UnityEngine;

public class S2_MBoss_AVaSTE : S2_BossBaseClass
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetUpBehaviors(0, behavior.lockOn, behavior.chase, behavior.disruptor, behavior.coverage);
        SetUpBehaviors(1, behavior.lockOn, behavior.chase, behavior.coverage, behavior.disruptor);
        SetUpBehaviors(2, behavior.lockOn, behavior.coverage, behavior.chase, behavior.disruptor);
        SetUpBehaviors(3, behavior.coverage, behavior.lockOn, behavior.chase, behavior.disruptor);
    }

    public override void Update()
    {
        base.Update();       
    }

    [SerializeField] Transform[] flourishPoints;
    private int nextPoint = 0;    
    Vector3 target;
    [SerializeField] float waitTime;
    bool waiting;

   
    public override void Flourish()
    {
        base.Flourish();
        
        //will in a clockwise pattern
        target = flourishPoints[nextPoint].position;
        childShip.transform.position = Vector3.MoveTowards(childShip.transform.position, target, stats.GetMinSpeed() * Time.deltaTime);

        if (Vector3.Distance(childShip.transform.position, target) < 0.5f)
        {
            if (!waiting)
            {
                waiting = true;
                Invoke(nameof(Wait), waitTime);
            }
        }
    }

    public void Wait()
    {
        if (nextPoint < flourishPoints.Length -1)       
            nextPoint++;    
        else
            nextPoint = 0;

        waiting = false;
    }

    #region Vulnerability Switching
    public override void OnStateChange()
    {
        base.OnStateChange();
        if (currentHealth == healthState.full)
        {
            weakPoints[0].SetVulnerablility(true);
            weakPoints[1].SetVulnerablility(true);
            weakPoints[2].SetVulnerablility(false);
            weakPoints[3].SetVulnerablility(false);
        }
        else if(currentHealth == healthState.threeQuater)
        {
            weakPoints[0].SetVulnerablility(true);
            weakPoints[1].SetVulnerablility(false);
            weakPoints[2].SetVulnerablility(true);
            weakPoints[3].SetVulnerablility(true);
            if(leftDmg > 8)
                weakPoints[2].SetVulnerablility(false);
            if(rightDmg > 8)
                weakPoints[3].SetVulnerablility(false);
        }
        else if(currentHealth == healthState.half)
        {
            weakPoints[0].SetVulnerablility(true);
            if(eyeVulnerable)
                weakPoints[1].SetVulnerablility(true);
            else
                weakPoints[1].SetVulnerablility(false);

            weakPoints[2].SetVulnerablility(false);
            weakPoints[3].SetVulnerablility(false);
        }
        else if(currentHealth == healthState.quater)
        {
            weakPoints[0].SetVulnerablility(true);
            weakPoints[1].SetVulnerablility(false);
            if(leftVulnerability)
                weakPoints[2].SetVulnerablility(true);
            else
                weakPoints[2].SetVulnerablility(false);
            if(rightVulnerability)
                weakPoints[3].SetVulnerablility(true);
            else
                weakPoints[3].SetVulnerablility(false);
        }        
    }

    private int leftDmg;
    private int rightDmg;
    private bool eyeVulnerable;
    private int eyeDmg;
    private int maxEyeDmg;
    private bool leftVulnerability;
    private bool rightVulnerability;
    private int ltDmg;
    private int rtDmg;

    public override void TakeDamage(int dmg, bool weak, S2_BossWeakPoint weakPoint)
    {
        base.TakeDamage(dmg, weak, weakPoint);
        if (!weak)
            return;
        else
        {
            if (currentHealth == healthState.threeQuater)
            {
                if (weakPoint == weakPoints[2])
                {
                    leftDmg++;
                    if (leftDmg > 8)
                    {
                        weakPoints[2].SetVulnerablility(false);
                    }
                }
                if (weakPoint == weakPoints[3])
                {
                    rightDmg++;
                    if (rightDmg > 8)
                    {
                        weakPoints[3].SetVulnerablility(false);
                    }
                }
            }
            else if (currentHealth == healthState.half)
            {
                if (weakPoint == weakPoints[1])
                {
                    eyeDmg++;
                    if (eyeDmg >= maxEyeDmg)
                    {
                        eyeVulnerable = false;
                        weakPoints[1].SetVulnerablility(false);
                        Invoke(nameof(EyeSwitch), 5);
                    }
                }
            }
            else if (currentHealth == healthState.quater)
            {
                if (weakPoint == weakPoints[2])
                {
                    ltDmg++;
                    if (ltDmg >= 3)
                    {
                        weakPoints[2].SetVulnerablility(false);
                        weakPoints[3].SetVulnerablility(true);
                        ltDmg = 0;
                        rtDmg = 0;
                    }
                }
                else if (weakPoint == weakPoints[3])
                {
                    rtDmg++;
                    if (rtDmg >= 3)
                    {
                        weakPoints[2].SetVulnerablility(true);
                        weakPoints[3].SetVulnerablility(false);
                        rtDmg = 0;
                        ltDmg = 0;
                    }
                }
            }
        }
    }

    public void EyeSwitch()
    {
        eyeVulnerable = true;
        weakPoints[1].SetVulnerablility(true);
        maxEyeDmg += 5;
    }
    #endregion

    public override void ResetBoss()
    {
        base.ResetBoss();
        nextPoint = 0;
        leftDmg = 0;
        rightDmg = 0;
        eyeVulnerable = true;
        maxEyeDmg = 5;
        rightVulnerability = false;
        leftVulnerability = true;
        ltDmg = 0;
        rtDmg = 0;

        weakPoints[0].SetVulnerablility(true);
        weakPoints[1].SetVulnerablility(true);
        weakPoints[2].SetVulnerablility(false);
        weakPoints[3].SetVulnerablility(false);

    }

}
