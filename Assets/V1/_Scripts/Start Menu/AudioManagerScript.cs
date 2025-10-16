using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    private Coroutine bgmFadeCoroutine;
    [SerializeField] private float bgmFadeDuration = 1.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        // volumeSettings = FindObjectOfType<VolumeSettings>();

        bgmSource = GameObject.Find("Music").GetComponent<AudioSource>();
        sfxSource = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            if (worldTime == null)
            {
                worldTime = FindFirstObjectByType<WorldTime>();
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

        AudioClip targetClip = isDay ? sunnyBgm : nightBgm;
        if (targetClip == null)
        {
            Debug.LogWarning("[AudioManager] No valid BGM clip found for current time.");
            return;
        }

        if (bgmSource.clip == targetClip && bgmSource.isPlaying)
            return; // Already playing the correct clip

        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(FadeToBgm(targetClip, bgmFadeDuration));
    }

    private IEnumerator FadeToBgm(AudioClip newClip, float duration)
    {
        float startVolume = bgmSource.volume;
        float targetVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        // Fade out
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        bgmSource.volume = 0f;

        // Switch clip
        bgmSource.clip = newClip;
        bgmSource.Play();

        // Fade in
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }
        bgmSource.volume = targetVolume;
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
