using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStatistics : MonoBehaviour
{
    public static ShipStatistics Instance { get; private set; }

    [SerializeField] protected ShipBase stats = null;
    [SerializeField] ParticleSystem[] engines;

    public int GetEngineLength()
    {
        return engines.Length;
    }
    private void Start()
    {
        engines = GetComponentsInChildren<ParticleSystem>();
    }
    void OnEnable() { Instance = this; if (ShipStatisticsController.Instance) ShipStatisticsController.Instance.GetNewValues(); }

    void OnDestroy() { Instance = null; }
    void OnDisable() { Instance = null; }

    public ShipBase GetStats() { return stats; }

    public void TurnOnEngines()
    {
        if (engines.Length > 0)
        {
            for (int i = 0; i < engines.Length; i++)
            {
                engines[i].Play();
            }
        }
    }

    public void TurnOffEngines()
    {
        if (engines.Length > 0)
        {
            for (int i = 0; i < engines.Length; i++)
            {
                engines[i].Stop();
            }
        }
    }
}
