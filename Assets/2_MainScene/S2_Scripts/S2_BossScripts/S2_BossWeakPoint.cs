using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossWeakPoint : MonoBehaviour
{
    [SerializeField]
    S2_BossBaseClass bossBody;


    [SerializeField]
    bool vulnerable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14 && vulnerable)
        {
            bossBody.TakeDamage(ShipStats.Instance.GetStats().GetAttributes()[0]);
        }
    }

    public void SetVulnerablility(bool b)
    {
        vulnerable = b;
    }
}
