using System;
using System.Collections;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChanged;

    [SerializeField] private float _dayLength; // Length of a day in seconds
    private TimeSpan _currentTime;
    private float _minuteLength => _dayLength / WorldTimeConstraints.MinutesInDay; // 1440 minutes in a day

    void Start()
    {
        StartCoroutine(AddMinutes());
    }

    private IEnumerator AddMinutes()
    {
        _currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChanged?.Invoke(this, _currentTime);
        yield return new WaitForSeconds(_minuteLength);
        StartCoroutine(AddMinutes());
    }
}
