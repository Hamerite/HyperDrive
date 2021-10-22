using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class S2_LockOnStats : ScriptableObject
{
    [SerializeField] protected float speed;
    [SerializeField] protected float lateralSpeed;
    [SerializeField] protected float timeOnScreen;

    public float GetSpeed() { return speed; }
    public float GetLateralSpeed() { return lateralSpeed; }
    public float GetTimeOnScreen() { return timeOnScreen; }
    
}
