//Created by Dylan LeClair 04/07/21
//Last modified 04/07/21 (Dylan LeClair)
//Modified 10/20/21 (Kyle Ennis)

using UnityEngine;

[CreateAssetMenu]
public class S2_EnemyBaseClass : ScriptableObject {
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected int[] attributes; //{ FirePower, Shields, Health }
    [SerializeField] protected int pointsValue;
    public float GetMaxSpeed() { return maxSpeed; }

    public int [] GetAttributes() { return attributes; }
}
