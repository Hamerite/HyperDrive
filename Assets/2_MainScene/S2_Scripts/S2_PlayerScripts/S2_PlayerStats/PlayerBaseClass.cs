//Created by Dylan LeClair 12/06/21
//Last modified 12/06/21 (Dylan LeClair)
using UnityEngine;

[CreateAssetMenu]
public class PlayerBaseClass : ScriptableObject {
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected int[] attributes; //{ firePower, shields, health, thrusters, regen, passives }

    public float GetMaxSpeed() { return maxSpeed; }

    public int[] GetAttributes() { return attributes; }
}
