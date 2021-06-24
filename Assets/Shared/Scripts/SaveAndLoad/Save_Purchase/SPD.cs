[System.Serializable]
public class SPD {
    public bool[] wasPurchased;
    public int shipSelected;

    public SPD(S1_ShipSelect s1_ShipSelect) {
        wasPurchased = s1_ShipSelect.GetWasPurchsed();
        shipSelected = s1_ShipSelect.GetIndex();
    }
}
