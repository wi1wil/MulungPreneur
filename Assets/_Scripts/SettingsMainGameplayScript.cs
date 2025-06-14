using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMainGameplayScript : MonoBehaviour
{
    GameObject audioManager;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    Slider musicSlider;
    Slider soundSlider;

    private void Awake()
    {
        audioManager = GameObject.Find("Audio Manager");

        if(audioManager)
        {
            bgmSource = GameObject.Find("Music").GetComponent<AudioSource>();
            sfxSource = GameObject.Find("SFX").GetComponent<AudioSource>();
        }

        if(!musicSlider)
            musicSlider = GameObject.Find("Music Slider").GetComponent<Slider>();
        if (!soundSlider)
            soundSlider = GameObject.Find("Sound Slider").GetComponent<Slider>();
    }
    private void Start()
    {
        musicSlider.value = bgmSource.volume;
        soundSlider.value = sfxSource.volume;
        musicSlider.onValueChanged.AddListener(UpdateBGMVolume);
        soundSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    void UpdateBGMVolume(float value)
    {
       bgmSource.volume = value;
    }
    void UpdateSFXVolume(float value)
    {
        sfxSource.volume = value;
    }
}
