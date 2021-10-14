//Created by Dylan LeClair 26/05/21
//Last modified 26/05/21 (Dylan LeClair)
[System.Serializable]
public class SPD {
    public bool[] wasPurchased;
    public int shipSelected;

    public SPD(S5_ShipSelect s5_ShipSelect) {
        wasPurchased = s5_ShipSelect.GetWasPurchsed();
        shipSelected = s5_ShipSelect.GetIndex();
    }
}
