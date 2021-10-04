//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    protected int[] scoringStats = new int[5]; //{ Score, Counter, enemiesKilled, Difficulty, Coins }

    void Awake() {
        if (Instance != null && Instance != this) DestroyImmediate(gameObject);
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SGD data = GDSM.LoadData();
        if (data == null) return;

        scoringStats[4] = data.coins;
    }

    public void SetScoreVariables(int[] runValues) { for (int i = 0; i < runValues.Length - 2; i++) scoringStats[i] = runValues[i]; }

    public void CalculateCoins() {
        scoringStats[4] += (scoringStats[0] * scoringStats[3]) + (scoringStats[1] * 2) + (scoringStats[2] * 2);
        SaveGameManager();
    }

    void SaveGameManager() { GDSM.SaveData(this); }

    public void SetCoinAmount(int amount) { scoringStats[4] = amount; }

    public int GetCoinAmount() { return scoringStats[4]; }

    public int GetScoreAmount() { return scoringStats[0]; }

    public void TraverseScenes(string sceneName) { SceneManager.LoadScene(sceneName, LoadSceneMode.Single); }
}