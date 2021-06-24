//Created by Dylan LeClair 12/06/21
//Last modified 12/06/21 (Dylan LeClair)
using UnityEngine;

public class PlayerBaseClass : MonoBehaviour {
    [SerializeField] protected float maxSpeed, firePower;
    [SerializeField] protected int shields;

    public virtual float GetMaxSpeed() { return maxSpeed; }

    public virtual float GetFirePower() { return firePower; }

    public virtual int GetSheilds() { return shields; }
}
