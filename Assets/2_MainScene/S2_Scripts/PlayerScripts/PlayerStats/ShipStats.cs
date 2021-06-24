//Created by Dylan Leclair 12/06/21
//Last modified 12/06/21 (Dylan LeClair)

public class ShipStats : PlayerBaseClass { 
    public static ShipStats Instance { get; private set; }

    void Awake() { Instance = this; }

    void OnDestroy() { Instance = null; }
}
