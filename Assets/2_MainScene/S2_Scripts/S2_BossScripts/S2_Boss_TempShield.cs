//Created by Alec Typelo
//Last modified 15/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_Boss_TempShield : MonoBehaviour {
    [SerializeField] protected S2_BossBaseClass boss;

    [SerializeField] protected int health;

    protected int damage;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 14) {
            damage++;
            if (damage >= health) {
                boss.DestroyShield();
                gameObject.SetActive(false);
            }
        }
    }
}
