using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S2_BossManager : MonoBehaviour
{
    public static S2_BossManager Instance { get; private set;}
       
    [SerializeField]
    GameObject[] bosses;
    S2_BossBaseClass activeBoss;
    [SerializeField] int bossNum;
    public int GetBossNum(){ return bossNum; }

    [SerializeField] GameObject bulletPooler;

    //these will be bars for the furture
    [SerializeField]
    GameObject bossCanvas;
    [SerializeField]
    Text healthText;
    [SerializeField]
    Text shieldTest;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        bossNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBoss(int i)
    {
        if (i < bosses.Length)
            bossNum = i;
        else
            bossNum = bosses.Length - 1;
        S2_PoolController.Instance.StopAsteroids();
        Invoke(nameof(BossWait), 0);
        bulletPooler.SetActive(true);
    }

    public void BossWait()
    {
        bosses[bossNum].SetActive(true);
        activeBoss = bosses[bossNum].GetComponentInChildren<S2_BossBaseClass>();
        activeBoss.ResetBoss();
        bossCanvas.SetActive(true);
    }

    public void EndBoss()
    {
        activeBoss.AddScore();
        bossCanvas.SetActive(true);
        bosses[bossNum].SetActive(false);
        S2_PoolController.Instance.StartUpAsteroids();
        bossCanvas.SetActive(false);
        bulletPooler.SetActive(false);
    }

    public void UpdateText(float health, float shield)
    {
        healthText.text = health.ToString();
        shieldTest.text = shield.ToString();
    }

    

}
