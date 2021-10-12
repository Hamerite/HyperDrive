using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossDisruptorBase : MonoBehaviour
{
    [SerializeField]
    S2_DisruptorStats[] bulletStats;
    float timeOnScreen;
    float warningTime;

    [SerializeField]
    GameObject[] bulletTypes; //use this to set type of bullet

    //[SerializeField]
    //GameObject damageObject;
    [SerializeField]
    GameObject warningObject;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        //fix this
        for (int i = 0; i < bulletTypes.Length; i++)
        { bulletTypes[i].SetActive(false); }
        //SelectBullet(S2_BossManager.Instance.GetBossNum());
        //////////////////////////
        ShowWarning();
        Invoke(nameof(ActivateBullets), warningTime);
        Invoke(nameof(DeactivateBullets), warningTime + timeOnScreen);
        

        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();
        warningTime = bulletStats[S2_BossManager.Instance.GetBossNum()].GetWarningTime();
    }


    public void ShowWarning()
    {
        warningObject.SetActive(true);
        bulletTypes[S2_BossManager.Instance.GetBossNum()].SetActive(false);
        gameObject.layer = 0;
    }

    public void ActivateBullets()
    {
        warningObject.SetActive(false);
        bulletTypes[S2_BossManager.Instance.GetBossNum()].SetActive(true);
        gameObject.layer = 9;
    }

    public void DeactivateBullets()
    {
        gameObject.SetActive(false);
    }

    public void SelectBullet(int i)
    {
        bulletTypes[i].SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        //play whatever effects needed here
        if (other.gameObject.layer != 9 && gameObject.layer == 9)
            gameObject.SetActive(false);
    }

}
