using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyBulletPooler : MonoBehaviour
{
    public static S2_EnemyBulletPooler Instance { private set; get; }
    [SerializeField] protected List<S2_BulletScript> bullets = new List<S2_BulletScript>();
    [SerializeField] protected List<S2_BulletScript> inUse = new List<S2_BulletScript>();

    private void Awake()
    {
        Instance = this;
    }
  
    public void AddBulletToInUse(S2_BulletScript bullet)
    {
        inUse.Add(bullet);
        bullets.Remove(bullet);
        bullets.TrimExcess();
    }
    public void RemoveBulletFromInUse(S2_BulletScript bullet)
    {
        bullets.Add(bullet);
        inUse.Remove(bullet);
        inUse.TrimExcess();
    }
}
