//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_BossChaserBase : MonoBehaviour {
    [SerializeField] protected S2_ChaserStats[] bulletStats;
    [SerializeField] protected GameObject[] bulletTypes; //use this to set type of bullet

    protected float speed, timeOnScreen;

    void OnEnable() {
        for (int i = 0; i < bulletTypes.Length; i++) { bulletTypes[i].SetActive(false); }

        speed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetSpeed();
        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();
        SelectBullet(S2_BossManager.Instance.GetBossNum());
        Invoke(nameof(StopDisableBullet), timeOnScreen);
    }

    void FixedUpdate() { transform.position += speed * Time.fixedDeltaTime * transform.forward; }

    public void StopDisableBullet() { gameObject.SetActive(false); }

    public void SelectBullet(int i) { bulletTypes[i].SetActive(true); }

    void OnTriggerEnter(Collider other) {
        //play whatever effects needed here
        if(other.gameObject.layer != 9) { gameObject.SetActive(false); }
    }
}
