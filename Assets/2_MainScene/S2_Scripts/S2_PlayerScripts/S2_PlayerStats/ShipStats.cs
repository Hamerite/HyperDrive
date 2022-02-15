//Created by Dylan Leclair 12/06/21
//Last modified 22/09/21 (Kyle Ennis)
using UnityEngine;

public class ShipStats : MonoBehaviour { 
    public static ShipStats Instance { get; protected set; }

    [SerializeField] protected PlayerBaseClass stats;
    [SerializeField] protected Transform gunPosition;
    [SerializeField] protected ParticleSystem[] engines;

    public PlayerBaseClass GetStats() { return stats; }
    public Transform GetGunPosition() { return gunPosition; }

    void Awake() { Instance = this; } void OnEnable() { Instance = this; }

    void OnDestroy() { Instance = null; } void OnDisable() { Instance = null; }

    public void ToggleEngines(bool state) {
        for (int i = 0; i < engines.Length; i++) {
            if (state) { engines[i].Play(); }
            else { engines[i].Stop(); }
        }
    } 
}
