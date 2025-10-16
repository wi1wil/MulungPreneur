using UnityEngine;

public class StreetLampsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _streetLamps;
    private DayNightCycle _dayNightCycle;

    private void Awake()
    {
        _dayNightCycle = FindAnyObjectByType<DayNightCycle>();

        // Initialize lamp states based on current time
        if (_dayNightCycle.IsDaytime())
            TurnOffLamps();
        else
            TurnOnLamps();
    }

    private void OnEnable()
    {
        _dayNightCycle.OnNightTime += TurnOnLamps;
        _dayNightCycle.OnDayTime += TurnOffLamps;
    }

    private void OnDisable()
    {
        _dayNightCycle.OnNightTime -= TurnOnLamps;
        _dayNightCycle.OnDayTime -= TurnOffLamps;
    }

    private void TurnOnLamps()
    {
        Debug.Log("Turning on street lamps.");
        foreach (GameObject lamp in _streetLamps)
        {
            foreach (Transform child in lamp.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void TurnOffLamps()
    {
        Debug.Log("Turning off street lamps.");
        foreach (GameObject lamp in _streetLamps)
        {
            foreach (Transform child in lamp.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
