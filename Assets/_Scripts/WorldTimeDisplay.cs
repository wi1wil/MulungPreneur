using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class WorldTimeDisplay : MonoBehaviour
{
    [SerializeField] private WorldTime _worldTime;
    [SerializeField] private TextMeshProUGUI _timeText;

    private void Awake()
    {
        _timeText = GetComponent<TextMeshProUGUI>();
        _worldTime.WorldTimeChanged += OnWorldTimeChanged;
    }

    private void Oestroy()
    {
        _worldTime.WorldTimeChanged -= OnWorldTimeChanged;  
    }

    private void OnWorldTimeChanged(object sender, TimeSpan newTime)
    {
        _timeText.SetText(newTime.ToString(@"hh\:mm"));
    }
}
