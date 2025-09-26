using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsMainGameplayScript : MonoBehaviour
{
    GameObject audioManager;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    Slider musicSlider;
    Slider soundSlider;
    [SerializeField] GameObject audioManagerPrefab;
    VolumeSettings volumeSettings;

    private void Awake()
    {
        audioManager = GameObject.Find("Audio Manager");
        volumeSettings = FindFirstObjectByType<VolumeSettings>();
        volumeSettings.LoadVolume();
    }

    void Udate()
    {
        volumeSettings.LoadVolume();
    }

    private void OnEnable()
    {
        if (volumeSettings != null)
            volumeSettings.LoadVolume();
    }
}
