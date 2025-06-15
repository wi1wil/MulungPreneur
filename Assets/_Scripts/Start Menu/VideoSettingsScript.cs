using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.XR;

public class VideoSettingsScript : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public TMP_Dropdown resDropDown;

    Resolution[] resolutions;
    public bool isFullscreen;
    int selectedResolution;
    List<Resolution> selectedResolutionList = new List<Resolution>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!fullScreenToggle)
        {
            fullScreenToggle = GameObject.Find("Fullscreen-Toggle").GetComponent<Toggle>();
        }
        if (!resDropDown)
        {
            resDropDown = GameObject.Find("Resolution-Dropdown").GetComponent<TMP_Dropdown>();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("yes");
        GameObject newCanvas = GameObject.Find("SettingPage");

        if (newCanvas != null)
        {
            transform.SetParent(newCanvas.transform, worldPositionStays: false);
        }

        GameObject menuCanvas = GameObject.Find("Menu (Activate by Tab)");
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-34, 268);
        menuCanvas.SetActive(false);
    }

    private void Start()
    {
        isFullscreen = true;
        resolutions = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution resolution in resolutions)
        {
            newRes = resolution.width.ToString() + " x " + resolution.height.ToString();
            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(resolution);
            }
        }

        resDropDown.AddOptions(resolutionStringList);
    }

    public void ChangeResolution()
    {
        selectedResolution = resDropDown.value;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }

    public void ChangeFullScreen()
    {
        isFullscreen = fullScreenToggle.isOn;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }

    public void StartGame()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Main Gameplay");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", selectedResolution);
        PlayerPrefs.SetInt("FullScreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
        {
            selectedResolution = PlayerPrefs.GetInt("Resolution");
            resDropDown.value = selectedResolution;
            ChangeResolution();
        }

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            isFullscreen = PlayerPrefs.GetInt("FullScreen") == 1;
            fullScreenToggle.isOn = isFullscreen;
            ChangeFullScreen();
        }
    }
}
