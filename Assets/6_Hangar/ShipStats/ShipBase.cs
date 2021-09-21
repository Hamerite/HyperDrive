//Created by Dylan LeClair 12/06/21
//Last modified 12/06/21 (Dylan LeClair)
using UnityEngine;

[CreateAssetMenu]
public class ShipBase : ScriptableObject {
    [SerializeField] protected int[] attributes; //{ speed, firePower, shields, health, thrusters, regen, passives }
    [SerializeField] protected string description;

    public string GetInformation() { return description; }
    public int[] GetAttributes() { return attributes; }
}
