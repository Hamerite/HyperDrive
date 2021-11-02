using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossHomingBase : MonoBehaviour
{
    [SerializeField]
    S2_LockOnStats[] bulletStats;
    float speed;
    float lateralSpeed;
    float timeOnScreen;

    [SerializeField]
    GameObject[] bulletTypes; //use this to set type of bullet

    Transform player;   

    // Start is called before the first frame update
    void Start()
    {
        player = S2_PlayerController.Instance.transform;
    }

    private void OnEnable()
    {
        for (int i = 0; i < bulletTypes.Length; i++)
        { bulletTypes[i].SetActive(false); }
        SelectBullet(S2_BossManager.Instance.GetBossNum());
        speed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetSpeed();
        lateralSpeed = bulletStats[S2_BossManager.Instance.GetBossNum()].GetLateralSpeed();
        timeOnScreen = bulletStats[S2_BossManager.Instance.GetBossNum()].GetTimeOnScreen();
        Invoke(nameof(DisableBullet), timeOnScreen);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
        Vector3 lateralPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, lateralPos, lateralSpeed * Time.fixedDeltaTime);
        if (transform.position.z > player.position.z)
        {
            bulletTypes[S2_BossManager.Instance.GetBossNum()].transform.LookAt(player.position);
        }
    }

    public void DisableBullet()
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
        if (other.gameObject.layer != 9)
            gameObject.SetActive(false);
    }
}
