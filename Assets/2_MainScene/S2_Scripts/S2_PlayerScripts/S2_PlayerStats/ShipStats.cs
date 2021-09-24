//Created by Dylan Leclair 12/06/21
//Last modified 22/09/21 (Kyle Ennis)
using UnityEngine;

public class ShipStats : MonoBehaviour { 
    public static ShipStats Instance { get; private set; }

    [SerializeField] protected PlayerBaseClass stats = null;
    [SerializeField] protected Transform gunPosition = null;
    [SerializeField] protected ParticleSystem[] engines;

    void Awake() { Instance = this; } void OnEnable() { Instance = this; }

    void OnDestroy() { Instance = null; } void OnDisable() { Instance = null; }

    public void ToggleEngines(bool state) {
        foreach (ParticleSystem item in engines) {
            if (state) item.Play();
            else item.Stop();
        }
    }

    public PlayerBaseClass GetStats() { return stats; }

    public Transform GetGunPosition() { return gunPosition; }
}
