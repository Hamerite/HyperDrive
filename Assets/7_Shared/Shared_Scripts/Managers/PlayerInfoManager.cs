//Created by Dylan LeClair 29/09/21
//Last modified 29/09/21 (Dylan Leclair)
using System;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour {
    public static PlayerInfoManager Instance { get; private set; }

    public float TimePlayed { get; protected set; }
    public int[] MemberSinceValues { get; protected set; }
    public int[] IntegerValues { get; protected set; } //{ totalScore, totalObstaclesPassed, totalEnemiesKilled, difficultyValue, highestKillStreak }
    public string HighestDifficultyReached { get; protected set; }

    void Awake() { 
        Instance = this;

        MemberSinceValues = new int[3];
        IntegerValues = new int[5];
        HighestDifficultyReached = "None";
        SID data = IDSM.LoadData();
        if (data == null) {
            MemberSinceValues[0] = DateTime.Today.Day;
            MemberSinceValues[1] = DateTime.Today.Month;
            MemberSinceValues[2] = DateTime.Today.Year;
            return;
        }

        for (int i = 0; i < MemberSinceValues.Length; i++) MemberSinceValues[i] = data.memberSinceValues[i];
        TimePlayed = data.timePlayed;
        IntegerValues = data.integerValues;
        HighestDifficultyReached = data.highestDifficulty;
    }

    void OnApplicationQuit() {
        TimePlayed += Time.time;
        SaveInfo();
    }

    public void SetInfoValues(int[] integers, string difficulty) {
        for (int i = 0; i < integers.Length - 3; i++) IntegerValues[i] += integers[i];

        if(integers[3] > IntegerValues[3]) {
            IntegerValues[3] = integers[3];
            HighestDifficultyReached = difficulty;
        }

        if (integers[4] > IntegerValues[4]) IntegerValues[4] = integers[4];

        SaveInfo();
    }

    void SaveInfo() { IDSM.SaveData(this); }
}
