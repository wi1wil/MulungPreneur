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
    //public Slider bgmSlider;
    //public Slider sfxSlider;
    //[SerializeField] float currentBgmVolume;
    //[SerializeField] float currentSfxVolume;

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
    }

    private void Start()
    {
        bgmSource.clip = sunnyBgm;
        bgmSource.loop = true;
        bgmSource.Play();
        volumeSettings.LoadVolume();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public float GetBgmVolume()
    {
        float value;
        audioMixer.GetFloat("BGM", out value);
        return Mathf.Pow(10, value / 20f);
    }

    public float GetSfxVolume()
    {
        float value;
        audioMixer.GetFloat("SFX", out value);
        return Mathf.Pow(10, value / 20f);
    }

    public void SetBgmVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }
}
