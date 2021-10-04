//Created by Dylan LeClair 29/09/21
//Last modified 29/09/21 (Dylan LeClair)
[System.Serializable]
public class SID {
    public float timePlayed;
    public int[] memberSinceValues;
    public int[] integerValues;
    public string highestDifficulty;

    public SID(PlayerInfoManager playerInfoManager) {
        timePlayed = playerInfoManager.GetTimePlayed();
        integerValues = playerInfoManager.GetIntergerValues();
        highestDifficulty = playerInfoManager.GetHighestDifficulty();
        memberSinceValues = playerInfoManager.GetMemberSinceValues();
    }
}
