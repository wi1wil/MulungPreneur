using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManagerScript : MonoBehaviour
{
    [Header("---------------------- Audio Source ----------------------")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("----------------------- Audio Clip -----------------------")]
    public AudioClip pickUpTrash;
    public AudioClip craftItem;
    public AudioClip uiClick;
    public AudioClip sunnyBgm;
    public AudioClip nightBgm;
    public AudioClip walkDefault;
    public AudioClip walkOnWater;

    [Header("----------------------- Audio Slider -----------------------")]
    public AudioMixer audioMixer;

    public static AudioManagerScript instance;
    VolumeSettings volumeSettings;

    private WorldTime worldTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        volumeSettings = FindObjectOfType<VolumeSettings>();

        bgmSource = GameObject.Find("Music").GetComponent<AudioSource>();
        sfxSource = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            if (worldTime == null)
            {
                worldTime = FindObjectOfType<WorldTime>();
                worldTime.DayNightChanged += OnDayNightChanged;
            }
        }
    }

    private void OnDestroy()
    {
        if (worldTime != null)
            worldTime.DayNightChanged -= OnDayNightChanged;
    }
    

    private void Start()
    {
        bgmSource.clip = sunnyBgm;
        bgmSource.loop = true;
        bgmSource.Play();
        bgmSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        if (volumeSettings != null)
            volumeSettings.LoadVolume();
        if (worldTime != null)
        {
            OnDayNightChanged(worldTime.IsDaytime());
        }
    }

    private void OnDayNightChanged(bool isDay)
    {

        if (bgmSource == null) {
            return;
        }
        Debug.Log($"[AudioManager] OnDayNightChanged called. isDay: {isDay}");

        if (isDay && sunnyBgm != null)
        {
            Debug.Log("[AudioManager] Switching to sunnyBgm.");
            bgmSource.clip = sunnyBgm;
            bgmSource.Play();
        }
        else if (!isDay && nightBgm != null)
        {
            Debug.Log("[AudioManager] Switching to nightBgm.");
            bgmSource.clip = nightBgm;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("[AudioManager] No valid BGM clip found for current time.");
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
