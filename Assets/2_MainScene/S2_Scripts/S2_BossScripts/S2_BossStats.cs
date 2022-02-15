//Created 21/09/21 by AT
//Last modified 15/02/22 ~Dylan LeClair
using UnityEngine;

[CreateAssetMenu]
public class S2_BossStats : ScriptableObject {
    [SerializeField] protected float minSpeed, maxSpeed;
    [SerializeField] protected float[] attributes; //{ FirePower, RapidFireSpeed, Shields, Health }
    [SerializeField] protected int points;

    public float GetMaxSpeed() { return maxSpeed; }

    public float[] GetAttributes() { return attributes; }

    public float GetMinSpeed() { return minSpeed; }

    public int GetPointValue() { return points; }
}
