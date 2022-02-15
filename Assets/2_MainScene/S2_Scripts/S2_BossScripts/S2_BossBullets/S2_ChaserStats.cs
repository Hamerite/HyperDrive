//Created by Alec Typelo
//Last modified 14/02/22 ~Dylan LeClair
using UnityEngine;

[CreateAssetMenu]
public class S2_ChaserStats : ScriptableObject {
    [SerializeField] protected float speed, timeOnScreen;

    public float GetSpeed() { return speed; }

    public float GetTimeOnScreen() { return timeOnScreen; }
}
