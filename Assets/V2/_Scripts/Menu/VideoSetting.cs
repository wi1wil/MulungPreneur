using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class VideoSetting : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullscreenToggle;

    private bool isFullscreen;
    private Resolution[] _resolutions;
    private readonly List<Resolution> selectedResolutionList = new();
    private int selectedResolutionIdx;
    private bool isLoading = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {

        InitializeResolutions();
        LoadSettings();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_resolutionDropdown == null)
            _resolutionDropdown = GameObject.FindAnyObjectByType<TMP_Dropdown>();

        if (_fullscreenToggle == null)
            _fullscreenToggle = GameObject.FindAnyObjectByType<Toggle>();

        if (_resolutionDropdown && _fullscreenToggle)
            LoadSettings();
    }

    private void InitializeResolutions()
    {
        _resolutions = Screen.resolutions;
        List<string> options = new();
        string resLabel;

        foreach (Resolution res in _resolutions)
        {
            resLabel = res.width + " x " + res.height;
            if (!options.Contains(resLabel))
            {
                options.Add(resLabel);
                selectedResolutionList.Add(res);
            }
        }

        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(options);
    }

    public void OnResolutionChange()
    {
        if (isLoading) return;

        selectedResolutionIdx = _resolutionDropdown.value;
        ApplySettings();
        SaveSettings();
    }

    public void OnFullscreenToggle()
    {
        if (isLoading) return;

        isFullscreen = _fullscreenToggle.isOn;
        ApplySettings();
        SaveSettings();
    }

    private void ApplySettings()
    {
        Resolution res = selectedResolutionList[selectedResolutionIdx];
        Screen.SetResolution(res.width, res.height, isFullscreen);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", selectedResolutionIdx);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        isLoading = true;

        selectedResolutionIdx = PlayerPrefs.GetInt("ResolutionIndex", selectedResolutionList.Count - 1);
        isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        selectedResolutionIdx = Mathf.Clamp(selectedResolutionIdx, 0, selectedResolutionList.Count - 1);

        if (_resolutionDropdown)
            _resolutionDropdown.value = selectedResolutionIdx;

        if (_fullscreenToggle)
            _fullscreenToggle.isOn = isFullscreen;

        if (selectedResolutionList.Count > 0)
            ApplySettings();

        isLoading = false;
    }
}
