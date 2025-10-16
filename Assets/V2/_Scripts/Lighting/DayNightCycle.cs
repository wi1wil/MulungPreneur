using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float _dayLengthSeconds = 600f;
    [SerializeField] private int dayStartHour = 6;
    [SerializeField] private int nightStartHour = 18;

    [Header("Light Settings")]
    [SerializeField] private Light2D _worldLight;
    [SerializeField] private Gradient _lightColorGradient;

    [Header("UI Settings")]
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _dayText;

    private TimeSpan _currentTime = new TimeSpan(6, 0, 0);
    private int _currentDay = 1;
    private bool _isDaytime = true;

    private float _minuteLength => _dayLengthSeconds / 1440f; // 1440 minutes in a day

    public event Action OnDayTime;
    public event Action OnNightTime;

    private void Start()
    {
        StartCoroutine(UpdateTime());
        UpdateUI();
        UpdateLight();
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            _currentTime += TimeSpan.FromMinutes(1);

            if (_currentTime.TotalMinutes >= 1440)
            {
                _currentTime = TimeSpan.Zero;
                _currentDay++;
            }

            // Check day/night
           bool nowIsDay = _currentTime.Hours >= dayStartHour && _currentTime.Hours < nightStartHour;
            if (nowIsDay != _isDaytime)
            {
                _isDaytime = nowIsDay;
                Debug.Log($"[WorldTime] Day/Night changed. Now isDay: {_isDaytime}");

                if (_isDaytime)
                    OnDayTime?.Invoke();
                else
                    OnNightTime?.Invoke();
            }

            UpdateUI();
            UpdateLight();

            yield return new WaitForSecondsRealtime(_minuteLength);
        }
    }

    private void UpdateUI()
    {
        if (_timeText != null) _timeText.SetText(_currentTime.ToString(@"hh\:mm"));
        if (_dayText != null) _dayText.SetText($"DAY - {_currentDay}");
    }

    private void UpdateLight()
    {
        if (_worldLight != null && _lightColorGradient != null)
        {
            _worldLight.color = _lightColorGradient.Evaluate((float)_currentTime.TotalMinutes / 1440f);
        }
    }

    public bool IsDaytime() => _isDaytime;
    public int GetCurrentDay() => _currentDay;
    public long GetCurrentTimeTicks() => _currentTime.Ticks;

    public void SetWorldTime(int day, long timeTicks)
    {
        _currentDay = day;
        _currentTime = TimeSpan.FromTicks(timeTicks);
    }
}
