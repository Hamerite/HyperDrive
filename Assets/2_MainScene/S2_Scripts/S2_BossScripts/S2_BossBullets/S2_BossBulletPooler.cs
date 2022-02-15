//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using System.Collections.Generic;
using UnityEngine;

public class S2_BossBulletPooler : MonoBehaviour {
    public static S2_BossBulletPooler Instance { get; protected set; }

    [SerializeField] protected GameObject lockOnPrefab, chasePrefab, disruptorPrefab, coveragePrefab;

    [SerializeField] protected List<GameObject> lockOnBullets = new List<GameObject>();
    [SerializeField] protected List<GameObject> chaseBullets = new List<GameObject>();
    [SerializeField] protected List<GameObject> disruptorBullets = new List<GameObject>();
    [SerializeField] protected List<GameObject> coverageBullets = new List<GameObject>();

    void Awake() { 
        Instance = this;

        lockOnBullets.TrimExcess();
        chaseBullets.TrimExcess();
        disruptorBullets.TrimExcess();
        coverageBullets.TrimExcess();
    }

    public GameObject GetBulletOfType(string type) {
        List<GameObject> bulletArray = new List<GameObject>();
        GameObject bulletPrefab = null;
        switch (type) {
            case "LockOn": bulletArray = lockOnBullets; bulletPrefab = lockOnPrefab; break;
            case "Chase": bulletArray = chaseBullets; bulletPrefab = chasePrefab; break;
            case "Disruptor": bulletArray = disruptorBullets; bulletPrefab = disruptorPrefab; break;
            case "Coverage": bulletArray = coverageBullets; bulletPrefab = coveragePrefab; break;
            default: break;
        }

        for (int i = 0; i < bulletArray.Count; i++) {
            if (!bulletArray[i].activeInHierarchy) { return bulletArray[i]; }
        }

        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        lockOnBullets.Add(bullet);
        return bullet;
    } 
}
