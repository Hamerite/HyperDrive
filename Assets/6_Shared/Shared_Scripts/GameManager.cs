//Created by Dylan LeClair
//Last revised 19-09-20 (Dylan LeClair)
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    protected int coins;
    protected int score;
    protected int counter;

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

    public void SetScoreVariables(int scoreValue, int counterValue) {
        score = scoreValue;
        counter = counterValue;
    }

    public void CalculateCoins() {
        coins += (score * 5) + (counter * 2);
        SaveGameManager();
    }

    void SaveGameManager() { GDSM.SaveData(this); }

    public void SetCoinAmount(int amount) { coins = amount; }

    public int GetCoinAmount() { return coins; }

    public int GetScoreAmount() { return score; }

    public void TraverseScenes(string sceneName) { SceneManager.LoadScene(sceneName, LoadSceneMode.Single); }
}