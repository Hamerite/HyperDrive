[System.Serializable]
public class SSD {
    public int highScore;
    public string champName;

    public SSD(S3_GameOverManager s3_GameOverManager) {
        highScore = s3_GameOverManager.GetHighScore();
        champName = s3_GameOverManager.GetChampName();
    }
}
