//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

[CreateAssetMenu]
public class S2_DisruptorStats : ScriptableObject {
    [SerializeField] protected float timeOnScreen, warningTime;

    public float GetTimeOnScreen() { return timeOnScreen; }

    public float GetWarningTime() { return warningTime; }
}
