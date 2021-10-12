using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class S2_CoverageStats : ScriptableObject
{
    [SerializeField] float speed;
    [SerializeField] float timeOnScreen;

    public float GetSpeed() { return speed; }
    public float GetTimeOnScreen() { return timeOnScreen; }


}
