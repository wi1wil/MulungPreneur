using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; 

    [Header(" ------------------------------ Audio Sources ------------------------------")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header(" ------------------------------ Audio Clips ------------------------------")]
    [SerializeField] private AudioClip _pickUpTrash;
    [SerializeField] private AudioClip _craftItem;
    [SerializeField] private AudioClip _uiClick;
    [SerializeField] private AudioClip _sunnyBgm;
    [SerializeField] private AudioClip _nightBgm;
    [SerializeField] private AudioClip _walkDefault;
    [SerializeField] private AudioClip _sellingItem;
    [SerializeField] private AudioClip _questCompleted;
    [SerializeField] private AudioClip _invFull;

    [Header(" ------------------------------ References ------------------------------")]
    [SerializeField] private DayNightCycle _dayNightCycle;

    [Header(" ------------------------------ Audio Settings ------------------------------")]
    [SerializeField] private float _fadeDuration = 2f;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (_musicSource != null)
        {
            _musicSource.clip = _sunnyBgm;
            _musicSource.Play();
            _musicSource.loop = true;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _dayNightCycle = FindAnyObjectByType<DayNightCycle>();

        if (_dayNightCycle != null)
        {
            _dayNightCycle.OnDayTime += PlaySunnyBgm;
            _dayNightCycle.OnNightTime += PlayNightBgm;

            if (_dayNightCycle.IsDaytime())
                PlaySunnyBgm();
            else
                PlayNightBgm();
        }
    }

    public void PlaySunnyBgm() => FadeTo(_sunnyBgm);
    public void PlayNightBgm() => FadeTo(_nightBgm);

    private void FadeTo(AudioClip newClip)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeMusicCoroutine(newClip));
    }

    private IEnumerator FadeMusicCoroutine(AudioClip newClip)
    {
        if (_musicSource.clip == newClip)
            yield break;

        Debug.Log($"[AudioManager] Fading to new clip: {newClip.name}");
        float startVolume = _musicSource.volume;

        // Fade out
        for (float t = 0; t < _fadeDuration; t += Time.unscaledDeltaTime)
        {
            _musicSource.volume = Mathf.Lerp(startVolume, 0f, t / _fadeDuration);
            yield return null;
        }

        _musicSource.clip = newClip;
        _musicSource.Play();

        // Fade in
        for (float t = 0; t < _fadeDuration; t += Time.unscaledDeltaTime)
        {
            _musicSource.volume = Mathf.Lerp(0f, startVolume, t / _fadeDuration);
            yield return null;
        }

        _musicSource.volume = startVolume;
    }

    public void PlayPickUpTrash() => _sfxSource.PlayOneShot(_pickUpTrash);
    public void PlayCraftItem() => _sfxSource.PlayOneShot(_craftItem);
    public void PlayUIClick() => _sfxSource.PlayOneShot(_uiClick);
    public void PlayWalkSFX() => _sfxSource.PlayOneShot(_walkDefault);
    public void PlaySellingItem() => _sfxSource.PlayOneShot(_sellingItem);
    public void PlayQuestCompleted() => _sfxSource.PlayOneShot(_questCompleted);
    public void PlayInvFull() => _sfxSource.PlayOneShot(_invFull);
}
