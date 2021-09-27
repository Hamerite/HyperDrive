[System.Serializable]
public class SPD {
    public bool[] wasPurchased;
    public int shipSelected;

    public SPD(S5_ShipSelect s5_ShipSelect) {
        wasPurchased = s5_ShipSelect.GetWasPurchsed();
        shipSelected = s5_ShipSelect.GetIndex();
    }
}
