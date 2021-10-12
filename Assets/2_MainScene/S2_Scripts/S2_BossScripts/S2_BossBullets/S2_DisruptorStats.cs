using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class S2_DisruptorStats : ScriptableObject
{
    [SerializeField] protected float timeOnScreen;
    [SerializeField] protected float warningTime;

    public float GetTimeOnScreen() { return timeOnScreen; }
    public float GetWarningTime() { return warningTime; }
}
