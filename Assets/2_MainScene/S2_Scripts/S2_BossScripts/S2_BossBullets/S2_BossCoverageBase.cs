//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_BossCoverageBase : MonoBehaviour {
    [SerializeField] protected S2_CoverageStats[] bulletStats;
    [SerializeField] protected GameObject[] bulletTypes, waves; //use this to set type of bullet

    protected GameObject activeWave;   

    protected float speed, timeOnScreen;

    void OnEnable() {
        if (activeWave) { activeWave.SetActive(false); }
                
        speed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetSpeed();
        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();
        SelectBullet(S2_BossManager.Instance.GetBossNum());
        Invoke(nameof(Deactivate), timeOnScreen);

        int RNG = Random.Range(0, waves.Length);       
        activeWave = bulletTypes[S2_BossManager.Instance.GetBossNum()].transform.GetChild(RNG).gameObject;
        activeWave.SetActive(true);
    }

    void FixedUpdate() { transform.position += speed * Time.fixedDeltaTime * transform.forward; }

    void Deactivate() { if (gameObject.activeInHierarchy) { gameObject.SetActive(false); } }

    public void SelectBullet(int i) { for (int j = 0; j < waves.Length; j++ ) { bulletTypes[i].SetActive(true); } }  
}
