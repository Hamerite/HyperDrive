using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class S2_ChaserStats : ScriptableObject
{
    [SerializeField] protected float speed;
    [SerializeField] protected float timeOnScreen;

    public float GetSpeed() { return speed; }
    public float GetTimeOnScreen() { return timeOnScreen; }
    
}
