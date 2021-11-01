//created 21/09/21 (Alek Tepylo)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BossWeakPoint : MonoBehaviour
{
    [SerializeField]
    S2_BossBaseClass bossBody;


    [SerializeField]
    bool vulnerable;
    [SerializeField]
    bool weakPoint;

    [SerializeField]
    Material vulnerableMat = null;
    [SerializeField]
    Material nonVulnerableMat = null;
    [SerializeField]MeshRenderer rend;

    // Start is called before the first frame update
    void Awake()
    {
    }   

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14 && vulnerable)
        {
            bossBody.TakeDamage(ShipStats.Instance.GetStats().GetAttributes()[0], weakPoint, this);
        }
    }

    public void SetVulnerablility(bool b)
    {
        vulnerable = b;
        if(b)
        {
            if (weakPoint)
            {
                rend.material.SetColor("_Color", Color.white);
                //rend.material = vulnerableMat;
            }
        }
        else
        {
            if (weakPoint)
            {
                rend.material.SetColor("_Color", Color.black);
                //rend.material = nonVulnerableMat;
            }
        }
    }
}
