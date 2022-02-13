//Created by Dylan LeClair 26/05/21
//Last modified 26/05/21 (Dylan LeClair)
[System.Serializable]
public class SSD {
    public int highScore;
    public string champName;

    public SSD(S3_GameOverManager s3_GameOverManager) {
        highScore = s3_GameOverManager.HighScore;
        champName = s3_GameOverManager.ChampName;
    }
}
