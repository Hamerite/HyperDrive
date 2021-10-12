using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Boss_SwitchOrb : MonoBehaviour
{
    [SerializeField]int health;
    int damage;
    [SerializeField] S2_BossBaseClass boss;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        damage = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            damage++;
            if(damage >= health)
            {
                boss.OrbDestroyed();
                gameObject.SetActive(false);
            }
        }
    }
}
