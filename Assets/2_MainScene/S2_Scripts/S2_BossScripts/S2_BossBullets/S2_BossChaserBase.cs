using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossChaserBase : MonoBehaviour
{
    [SerializeField]
    S2_ChaserStats[] bulletStats;
    float speed;
    float timeOnScreen;

    [SerializeField]
    GameObject[] bulletTypes; //use this to set type of bullet

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        for (int i = 0; i < bulletTypes.Length; i++)
        { bulletTypes[i].SetActive(false); }
        SelectBullet(S2_BossManager.Instance.GetBossNum());
        speed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetSpeed();
        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();
        Invoke(nameof(StopDisableBullet), timeOnScreen);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }


    public virtual void Movement()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    public void StopDisableBullet()
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
        if(other.gameObject.layer != 9)
        {
            gameObject.SetActive(false);
        }
    }

}
