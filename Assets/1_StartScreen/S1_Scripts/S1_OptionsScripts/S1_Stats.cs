//Created by Dylan LeClair 30/09/21
//Last modified 30/09/21 (Dylan LeClair)
using System;
using UnityEngine;
using UnityEngine.UI;

public class S1_Stats : MonoBehaviour {
    [SerializeField] protected Text[] joinDates = null;
    [SerializeField] protected Text[] statsValues = null;

    void OnEnable() {
        joinDates[0].text = (PlayerInfoManager.Instance.GetMemberSinceValues()[0].ToString() + " : "
                                + PlayerInfoManager.Instance.GetMemberSinceValues()[1].ToString() + " : "
                                + PlayerInfoManager.Instance.GetMemberSinceValues()[2].ToString());

        DateTime startDate = new DateTime(PlayerInfoManager.Instance.GetMemberSinceValues()[2],
                                            PlayerInfoManager.Instance.GetMemberSinceValues()[1],
                                            PlayerInfoManager.Instance.GetMemberSinceValues()[0]);
        DateTime currentDate = DateTime.Today;
        double memberYears = Math.Floor((currentDate - startDate).TotalDays / 365);
        double memberMonths = Math.Floor(((((currentDate - startDate).TotalDays / 365) % 12) - memberYears) * 12);
        double memberDays = Math.Floor((currentDate - startDate).TotalDays - (memberYears * 365) - (memberMonths * 30));
        joinDates[1].text = (memberYears.ToString("0") + " y : " + memberMonths.ToString("00") + " m : " + memberDays.ToString("00") + " d");

        string seconds = Math.Floor((PlayerInfoManager.Instance.GetTimePlayed()) % 60).ToString("00");
        string minutes = Math.Floor((PlayerInfoManager.Instance.GetTimePlayed() / 60) % 60).ToString("00");
        string hours = Math.Floor((PlayerInfoManager.Instance.GetTimePlayed() / 3600) % 24).ToString("00");
        string days = Math.Floor(PlayerInfoManager.Instance.GetTimePlayed() / 86400).ToString("0");
        statsValues[0].text = (days + " d : " + hours + " h : " + minutes + " m : " + seconds + " s");

        for (int i = 1; i < statsValues.Length - 2; i++) statsValues[i].text = PlayerInfoManager.Instance.GetIntergerValues()[i-1].ToString();
        statsValues[4].text = PlayerInfoManager.Instance.GetIntergerValues()[4].ToString();
        statsValues[5].text = PlayerInfoManager.Instance.GetHighestDifficulty();
    }
}
