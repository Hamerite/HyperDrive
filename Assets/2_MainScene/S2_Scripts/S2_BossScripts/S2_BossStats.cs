//Created 21/09/21 by AT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class S2_BossStats : ScriptableObject
{
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float minSpeed;
    [SerializeField] protected float[] attributes; //{ FirePower, RapidFireSpeed, Shields, Health }
    [SerializeField] protected float points;
    public float GetMaxSpeed() { return maxSpeed; }

    public float[] GetAttributes() { return attributes; }

    public float GetMinSpeed() { return minSpeed; }
    public float GetPointValue() { return points; }
}
