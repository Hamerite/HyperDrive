using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossCoverageBase : MonoBehaviour
{
    [SerializeField]
    S2_CoverageStats[] bulletStats;
    [SerializeField]
    float speed;
    [SerializeField]
    float timeOnScreen;

    [SerializeField]
    GameObject[] bulletTypes; //use this to set type of bullet

    [SerializeField]
    GameObject[] waves;

    GameObject activeWave;   

    private void OnEnable()
    {
        for(int i = 0; i < bulletTypes.Length;i++)
        { bulletTypes[i].SetActive(false); }
        SelectBullet(S2_BossManager.Instance.GetBossNum());
        
        for(int i = 0; i < bulletTypes[S2_BossManager.Instance.GetBossNum()].transform.childCount; i++)
        { bulletTypes[S2_BossManager.Instance.GetBossNum()].transform.GetChild(i).gameObject.SetActive(false); }
                
        speed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetSpeed();
        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();

        if (activeWave)
            activeWave.SetActive(false);

        Invoke(nameof(Deactivate), timeOnScreen);
        int r = Random.Range(0, waves.Length);       
        activeWave = bulletTypes[S2_BossManager.Instance.GetBossNum()].transform.GetChild(r).gameObject;
        activeWave.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    void Deactivate()
    {     
        if(gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }

    public void SelectBullet(int i)
    {
        for (int j = 0; j < waves.Length; j++ )
        {
            bulletTypes[i].SetActive(true);
        }
    }  


}
