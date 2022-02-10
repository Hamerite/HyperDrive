//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
//Modified 10/20/21 (Kyle Ennis)

using UnityEngine;
using System.Collections;
using System;


[CreateAssetMenu]
public class S2_EnemyBaseStats : ScriptableObject {
    [SerializeField] protected float maxSpeed, shotTime;
    [SerializeField] protected int firePower, shields, health, pointsValue;

    public float GetMaxSpeed() { return maxSpeed; }
    public float GetShotTime() { return shotTime; }
    public int GetFirePower() { return firePower; }
    public int GetShields() { return shields; }
    public int GetHealth() { return health; }
    public int GetPointsValue() { return pointsValue; }

}
