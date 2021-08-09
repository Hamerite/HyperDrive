using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_BulletPooler : MonoBehaviour {
    public static S2_BulletPooler Instance { get; private set; }

    [SerializeField] protected GameObject bulletPrefab = null;
    [SerializeField] protected List<GameObject> bullets = new List<GameObject>();

    void Awake() { Instance = this; }

    public GameObject GetBullet() {
        for (int i = 0; i < bullets.Count; i++) if (!bullets[i].activeInHierarchy) return bullets[i];

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bullets.Add(bullet);
        return bullet;
    }
}
