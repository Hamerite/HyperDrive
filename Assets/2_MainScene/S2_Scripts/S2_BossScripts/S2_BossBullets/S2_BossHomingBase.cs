//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_BossHomingBase : MonoBehaviour {
    [SerializeField] protected S2_LockOnStats[] bulletStats;
    [SerializeField] protected GameObject[] bulletTypes; //use this to set type of bullet
    
    protected float speed, lateralSpeed, timeOnScreen;

    void OnEnable() {
        for (int i = 0; i < bulletTypes.Length; i++) { bulletTypes[i].SetActive(false); }

        speed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetSpeed();
        lateralSpeed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetLateralSpeed();
        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();
        SelectBullet(S2_BossManager.Instance.GetBossNum());
        Invoke(nameof(DisableBullet), timeOnScreen);
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
        Vector3 lateralPos = new Vector3(S2_PlayerController.Instance.transform.position.x, S2_PlayerController.Instance.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, lateralPos, lateralSpeed * Time.fixedDeltaTime);

        if (transform.position.z > S2_PlayerController.Instance.transform.position.z) { bulletTypes[S2_BossManager.Instance.GetBossNum()].transform.LookAt(S2_PlayerController.Instance.transform.position); }
    }

    public void DisableBullet() { gameObject.SetActive(false); }

    public void SelectBullet(int i) { bulletTypes[i].SetActive(true); }

    void OnTriggerEnter(Collider other) {
        //play whatever effects needed here
        if (other.gameObject.layer != 9) { gameObject.SetActive(false); }     
    }
}
