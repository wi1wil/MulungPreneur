using System;
using System.Collections;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChanged;

    [SerializeField] private float _dayLength; // Length of a day in seconds
    private TimeSpan _currentTime = new TimeSpan(6, 0, 0); // Start at 6AM
    private float _minuteLength => _dayLength / WorldTimeConstraints.MinutesInDay; // 1440 minutes in a day

    public int _currentDay { get; private set; } = 1; // Start at day 1
    
    public int getCurrentDay() => _currentDay;
    public long getCurrentTimeTicks() => _currentTime.Ticks;

    void Start()
    {
        // Do not start the coroutine here.
        // The save/load system will call StartWorldTime() after loading.
    }

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

        WorldTimeChanged?.Invoke(this, _currentTime);
        yield return new WaitForSeconds(_minuteLength);
        StartCoroutine(AddMinutes());
    }
}
