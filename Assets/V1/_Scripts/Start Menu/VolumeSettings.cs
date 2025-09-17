using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Awake()
    {
        getSlider();

        if (!PlayerPrefs.HasKey("MusicVolume") || !PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
            PlayerPrefs.Save();
        }
        LoadVolume();
    }

    void Update()
    {
        getSlider();
    }

    public void getSlider()
    {
        bgmSlider = GameObject.FindGameObjectWithTag("MusicSlider")?.GetComponent<Slider>();
        if (bgmSlider == null)
        {
            bgmSlider = GameObject.Find("Music Slider")?.GetComponent<Slider>();
        }
        sfxSlider = GameObject.FindGameObjectWithTag("SoundSlider")?.GetComponent<Slider>();
        if (sfxSlider == null)
        {
            sfxSlider = GameObject.Find("Sound Slider")?.GetComponent<Slider>();
        }

        // Remove previous listeners to avoid stacking
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(delegate { SetMusicVolume(); });
            bgmSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(delegate { SetSFXVolume(); });
            sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
        }

        // Set slider values to saved PlayerPrefs after finding them
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        if (bgmSlider != null)
            bgmSlider.value = musicVol;
        if (sfxSlider != null)
            sfxSlider.value = sfxVol;
    }

    public void SetMusicVolume()
    {
        if (bgmSlider == null) return;
        float volume = bgmSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        if (sfxSlider == null) return;
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        if (bgmSlider != null)
        {
            bgmSlider.value = musicVol;
            audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(musicVol, 0.0001f)) * 20);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVol;
            audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(sfxVol, 0.0001f)) * 20);
        }
    }

    public void SetSliders(Slider bgm, Slider sfx)
    {
        bgmSlider = bgm;
        sfxSlider = sfx;
        LoadVolume();
    }
}
