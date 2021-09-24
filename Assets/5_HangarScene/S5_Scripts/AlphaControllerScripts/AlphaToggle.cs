//Created by Kyle Ennis 13/09/21
//Last modified 22/09/21 (Dylan LeClair)
using UnityEngine;

public class AlphaToggle : MonoBehaviour {
    public void ToggleAlpha() { S5_ShipSelectController.Instance.SetSlideAlpha(false); }

    public void CastToEnginesOn() { ShipStats.Instance.ToggleEngines(true); }

    public void CastToEnginesOff() { ShipStats.Instance.ToggleEngines(false); }
}
