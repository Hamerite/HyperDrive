using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyBulletPooler : MonoBehaviour
{
    public static S2_EnemyBulletPooler Instance { private set; get; }
    [SerializeField] protected List<S2_BulletScript> bullets = new List<S2_BulletScript>();
    public List<S2_BulletScript> GetBullets()
    {
        return bullets;
    }
    [SerializeField] protected List<S2_BulletScript> inUse = new List<S2_BulletScript>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddBulletToInUse(S2_BulletScript bullet)
    {
        bullets.Remove(bullet);
        inUse.Add(bullet);
    }
    public void RemoveBulletFromInUse(S2_BulletScript bullet)
    {
        inUse.Remove(bullet);
        bullets.Add(bullet);
    }
}
