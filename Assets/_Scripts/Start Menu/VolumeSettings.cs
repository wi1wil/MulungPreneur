using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        if (PlayerPrefs.HasKey("MusicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    private void Update()
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
    }

    public void SetMusicVolume()
    {
        float volume = bgmSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }   

    public void LoadVolume()
    {
        if (bgmSlider != null)
        {
            bgmSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SetMusicVolume();
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            SetSFXVolume();
        }
    }
}
