//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

[CreateAssetMenu]
public class S2_LockOnStats : ScriptableObject {
    [SerializeField] protected float speed, lateralSpeed, timeOnScreen;

    public float GetSpeed() { return speed; }

    public float GetLateralSpeed() { return lateralSpeed; }

    public float GetTimeOnScreen() { return timeOnScreen; }
}
