//Created by Dylan LeClair 12/06/21
//Last modified 22/09/21 (Kyle Ennis)
//Modified 10/20/21 (Kyle Ennis)
using UnityEngine;

[CreateAssetMenu]
public class PlayerBaseClass : ScriptableObject {
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected int[] attributes; //{ firePower, shields, health, thrusters, regen, passives }
    [SerializeField] protected string description;

    public float GetMaxSpeed() { return maxSpeed; }

    public int[] GetAttributes() { return attributes; }

    public string GetDescription() { return description; }
}
