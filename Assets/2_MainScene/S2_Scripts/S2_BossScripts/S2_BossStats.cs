//Created 21/09/21 AT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class S2_BossStats : ScriptableObject
{
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected int[] attributes; //{ FirePower, Shields, Health }

    public float GetMaxSpeed() { return maxSpeed; }

    public int[] GetAttributes() { return attributes; }
}
