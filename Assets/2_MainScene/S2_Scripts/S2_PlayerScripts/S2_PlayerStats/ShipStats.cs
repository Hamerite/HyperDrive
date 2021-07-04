//Created by Dylan Leclair 12/06/21
//Last modified 12/06/21 (Dylan LeClair)
using UnityEngine;

public class ShipStats : MonoBehaviour { 
    public static ShipStats Instance { get; private set; }

    [SerializeField] protected PlayerBaseClass stats = null;

    void Awake() { Instance = this; } void OnEnable() { Instance = this; }

    void OnDestroy() { Instance = null; } void OnDisable() { Instance = null; }

    public PlayerBaseClass GetStats() { return stats; }
}
