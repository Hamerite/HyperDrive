using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Boss_TempShield : MonoBehaviour
{
    [SerializeField]S2_BossBaseClass boss;
    [SerializeField]int health;
    private int damage;


    private void Awake()
    {
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            damage++;
            if (damage >= health)
            {
                boss.DestroyShield();
                gameObject.SetActive(false);
            }
        }
    }
}
