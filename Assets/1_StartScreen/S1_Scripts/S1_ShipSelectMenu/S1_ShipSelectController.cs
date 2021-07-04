//Created by Dylan LeClair 24/06/21
//Last Modified 24/06/21 (Dylan LeClair)
using UnityEngine;
using UnityEngine.UI;

public class S1_ShipSelectController : MonoBehaviour {
    public static S1_ShipSelectController Instance { get; private set; }

    [SerializeField] protected Slider[] statsSilders = null; // { Speed, FirePower, Shields, health, thrusters, regen }

    private void Awake() { Instance = this; }

    void Start() {
        if (!ShipStats.Instance) return;

        statsSilders[0].value = ShipStats.Instance.GetStats().GetMaxSpeed();
        for (int i = 1; i < statsSilders.Length; i++) statsSilders[i].value = ShipStats.Instance.GetStats().GetAttributes()[i - 1];
    }

    public void GetNewValues() { Invoke(nameof(Start), 0.1f); }
}
