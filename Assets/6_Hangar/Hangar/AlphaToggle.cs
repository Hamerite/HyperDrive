using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaToggle : MonoBehaviour
{
  public void ToggleAlpha()
    {
        ShipStatisticsController.Instance.slideAlpha = false;
    }
    public void CastToEnginesOn()
    {
        if (ShipStatistics.Instance.GetEngineLength() > 0)
        {
            ShipStatistics.Instance.TurnOnEngines();
        }
    }

    public void CastToEnginesOff()
    {
        if (ShipStatistics.Instance.GetEngineLength() > 0)
        {
            ShipStatistics.Instance.TurnOffEngines();
        }
    }
}
