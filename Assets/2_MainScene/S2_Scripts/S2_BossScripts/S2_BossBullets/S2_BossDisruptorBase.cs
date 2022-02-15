//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_BossDisruptorBase : MonoBehaviour {
    [SerializeField] protected S2_DisruptorStats[] bulletStats;
    [SerializeField] protected GameObject warningObject;
    [SerializeField] protected GameObject[] bulletTypes; //use this to set type of bullet

    protected float timeOnScreen, warningTime, timer;

    void OnEnable() {
        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();
        warningTime = bulletStats[S2_BossManager.Instance.GetBossNum()].GetWarningTime();

        ShowWarning();
        SelectBullet(S2_BossManager.Instance.GetBossNum());
        Invoke(nameof(ActivateBullets), warningTime);
        Invoke(nameof(DeactivateBullets), warningTime + timeOnScreen);

    }

    public void ShowWarning() {
        warningObject.SetActive(true);
        bulletTypes[S2_BossManager.Instance.GetBossNum()].SetActive(false);
        gameObject.layer = 0;
    }

    public void ActivateBullets() {
        warningObject.SetActive(false);
        bulletTypes[S2_BossManager.Instance.GetBossNum()].SetActive(true);
        gameObject.layer = 9;
    }

    public void DeactivateBullets() { gameObject.SetActive(false); }

    public void SelectBullet(int i) { bulletTypes[i].SetActive(true); }

    void OnTriggerEnter(Collider other) {
        //play whatever effects needed here
        if (other.gameObject.layer != 9 && gameObject.layer == 9) { gameObject.SetActive(false); }    
    }
}
