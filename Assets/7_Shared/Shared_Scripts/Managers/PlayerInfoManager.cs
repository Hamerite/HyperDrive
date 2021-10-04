//Created by Dylan LeClair 29/09/21
//Last modified 29/09/21 (Dylan Leclair)
using System;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour {
    public static PlayerInfoManager Instance { get; private set; }

    protected float timePlayed = 0;
    protected int[] memberSinceValues = new int[3];
    protected int[] integerValues = new int[5] { 0, 0, 0, 0, 0 }; //{ totalScore, totalObstaclesPassed, totalEnemiesKilled, difficultyValue, highestKillStreak }
    protected string highestDifficultyReached = "None";

    void Awake() { 
        Instance = this;

        SID data = IDSM.LoadData();
        if (data == null) {
            memberSinceValues[0] = DateTime.Today.Day;
            memberSinceValues[1] = DateTime.Today.Month;
            memberSinceValues[2] = DateTime.Today.Year;
            return;
        }

        for (int i = 0; i < memberSinceValues.Length; i++) memberSinceValues[i] = data.memberSinceValues[i];
        timePlayed = data.timePlayed;
        integerValues = data.integerValues;
        highestDifficultyReached = data.highestDifficulty;
    }

    void OnApplicationQuit() {
        timePlayed += Time.time;
        SaveInfo();
    }

    public void SetInfoValues(int[] integers, string difficulty) {
        for (int i = 0; i < integers.Length - 3; i++) integerValues[i] += integers[i];

        if(integers[3] > integerValues[3]) {
            integerValues[3] = integers[3];
            highestDifficultyReached = difficulty;
        }

        if (integers[4] > integerValues[4]) integerValues[4] = integers[4];

        SaveInfo();
    }

    public float GetTimePlayed() { return timePlayed; }

    public int[] GetIntergerValues() { return integerValues; }

    public string GetHighestDifficulty() { return highestDifficultyReached; }

    public int[] GetMemberSinceValues() { return memberSinceValues; }

    void SaveInfo() { IDSM.SaveData(this); }
}
