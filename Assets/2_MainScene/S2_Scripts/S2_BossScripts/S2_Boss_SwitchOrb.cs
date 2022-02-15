//Created by Alec Typelo
//Last modified 15/02/22 ~Dylan LeClair
using UnityEngine;

public class S2_Boss_SwitchOrb : MonoBehaviour {
    [SerializeField] protected S2_BossBaseClass boss;

    [SerializeField] protected int health;

    protected int damage;

    void OnEnable() { damage = 0; }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 14) {
            damage++;
            if(damage >= health) {
                boss.OrbDestroyed();
                gameObject.SetActive(false);
            }
        }
    }
}
