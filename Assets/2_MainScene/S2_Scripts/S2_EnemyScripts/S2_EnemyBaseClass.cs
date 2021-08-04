﻿//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
using UnityEngine;

[CreateAssetMenu]
public class S2_EnemyBaseClass : ScriptableObject {
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected int[] attributes; //{ FirePower, Shields, Health }

    public float GetMaxSpeed() { return maxSpeed; }

    public int [] GetAttributes() { return attributes; }
}