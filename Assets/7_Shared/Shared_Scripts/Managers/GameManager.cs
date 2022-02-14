//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public int[] ScoringStats { get; set; } //{ Score, Counter, enemiesKilled, Difficulty, Coins }

    void Awake() {
        if (Instance != null && Instance != this) DestroyImmediate(gameObject);
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        ScoringStats = new int[5];
        SGD data = GDSM.LoadData();
        if (data == null) return;

        ScoringStats[4] = data.coins;
    }

    public void SetScoreVariables(int[] runValues) { for (int i = 0; i < runValues.Length - 2; i++) ScoringStats[i] = runValues[i]; }

    public void CalculateCoins() {
        ScoringStats[4] += (ScoringStats[0] * ScoringStats[3]) + (ScoringStats[1] * 2) + (ScoringStats[2] * 2);
        GDSM.SaveData(this);
    }

    public void TraverseScenes(string sceneName) { SceneManager.LoadScene(sceneName, LoadSceneMode.Single); }
}