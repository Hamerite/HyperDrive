using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipStatUI : MonoBehaviour
{

    public static ShipStatUI Instance { get; private set; }

    [SerializeField] protected Slider[] statsSilders = null; // { Speed, FirePower, Shields, health, thrusters, regen }

    private void Awake() { Instance = this; }

    void Start()
    {
        if (!ShipStatistics.Instance) return;

        for (int i = 0; i < statsSilders.Length; i++) statsSilders[i].value = ShipStatistics.Instance.GetStats().GetAttributes()[i - 1];
    }

    public void GetNewValues() { Invoke(nameof(Start), 0.1f); }
}
