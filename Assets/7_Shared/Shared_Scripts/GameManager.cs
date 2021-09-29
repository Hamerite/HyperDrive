//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    protected int coins, score, difficulty, counter, enemiesKilled;

    void Awake() {
        if (Instance != null && Instance != this) DestroyImmediate(gameObject);
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SGD data = GDSM.LoadData();
        if (data == null) return;

        coins = data.coins;
    }

    public void SetScoreVariables(int scoreValue, int difficultyReached, int counterValue, int kills) {
        score = scoreValue;
        difficulty = difficultyReached;
        counter = counterValue;
        enemiesKilled = kills;
    }

    public void CalculateCoins() {
        coins += (score * difficulty) + (counter * 2) + (enemiesKilled * 2);
        SaveGameManager();
    }

    void SaveGameManager() { GDSM.SaveData(this); }

    public void SetCoinAmount(int amount) { coins = amount; }

    public int GetCoinAmount() { return coins; }

    public int GetScoreAmount() { return score; }

    public void TraverseScenes(string sceneName) { SceneManager.LoadScene(sceneName, LoadSceneMode.Single); }
}