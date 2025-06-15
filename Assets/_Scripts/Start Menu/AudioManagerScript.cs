using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
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

    private void Start()
    {
        bgmSource.clip = sunnyBgm;
        bgmSource.loop = true;
        bgmSource.Play();
        bgmSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        if (volumeSettings != null)
            volumeSettings.LoadVolume();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
