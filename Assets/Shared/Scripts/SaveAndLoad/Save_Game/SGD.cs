//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
[System.Serializable]
public class SGD{
    public int coins;

    public SGD(GameManager gameManager) { coins = gameManager.GetCoinAmount(); }
}
