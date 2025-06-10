using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        bgmSource.clip = sunnyBgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void setBGM(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    public void setSFX(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
