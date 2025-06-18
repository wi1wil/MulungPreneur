using System;
using System.Collections;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChanged;
    public event Action<bool> DayNightChanged; // true = isDay, false = isNight

    [SerializeField] private float _dayLength; // Length of a day in seconds
    private TimeSpan _currentTime = new TimeSpan(6, 0, 0); // Start at 6AM
    private float _minuteLength => _dayLength / WorldTimeConstraints.MinutesInDay; // 1440 minutes in a day

    public int _currentDay { get; private set; } = 1; // Start at day 1
    
    public int getCurrentDay() => _currentDay;
    public long getCurrentTimeTicks() => _currentTime.Ticks;

    private bool isDay = true;

    private int dayStartHour = 6;
    private int nightStartHour = 18;

    public void StartWorldTime()
    {
        StartCoroutine(AddMinutes());
    }

    public void setWorldTime(int day, long timeTicks)
    {
        _currentDay = day;
        _currentTime = new TimeSpan(timeTicks);
    }

    private IEnumerator AddMinutes()
    {
        _currentTime += TimeSpan.FromMinutes(1);
        if (_currentTime.TotalMinutes >= WorldTimeConstraints.MinutesInDay)
        {
            _currentTime = TimeSpan.Zero;
            _currentDay++;
        }

        // Check for day/night transition
        bool nowIsDay = _currentTime.Hours >= dayStartHour && _currentTime.Hours < nightStartHour;
        if (nowIsDay != isDay)
        {
            isDay = nowIsDay;
            Debug.Log($"[WorldTime] Day/Night changed. Now isDay: {isDay}");
            DayNightChanged?.Invoke(isDay);
        }

        WorldTimeChanged?.Invoke(this, _currentTime);
        yield return new WaitForSeconds(_minuteLength);
        StartCoroutine(AddMinutes());
    }

    public bool IsDaytime()
    {
        return isDay;
    }
}
