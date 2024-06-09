using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public int dayInGame = 1;

    public TextMeshProUGUI dayUI;

    public UnityEvent OnDayPass = new UnityEvent();

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public enum DayOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public Season currentSeason = Season.Spring;

    public DayOfWeek currentDayOfWeek = DayOfWeek.Monday;

    private int dayPerSeasons = 1;
    private int daysInCurrentSeasons = 1;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateUI();
    }
    public void TriggerNextDay()
    {
        dayInGame += 1;
        daysInCurrentSeasons += 1;

        currentDayOfWeek = (DayOfWeek)(((int)currentDayOfWeek + 1) % 7);

        OnDayPass.Invoke();


        if (daysInCurrentSeasons > 15)
        {
            daysInCurrentSeasons = 1;
            currentSeason = GetNextSeason();
        }

        UpdateUI();
    }

    private Season GetNextSeason()
    {
        int currentSeasonIndex = (int)currentSeason;
        int nextSeasonIndex = (currentSeasonIndex + 1) % 4;


        return (Season)nextSeasonIndex;
    }

    private void UpdateUI()
    {
        dayUI.text = $" Day: {dayInGame} {currentDayOfWeek}, {currentSeason}";
    }
}
