using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;

    private SwitchScene _switchScene;

    private void Start()
    {
        if (_settingsMenu != null)
            _settingsMenu.SetActive(false);
        if (_mainMenu != null)
            _mainMenu.SetActive(true);
        _switchScene = GetComponent<SwitchScene>();
    }

    public void OpenSettings()
    {
        AudioManager.instance.PlayUIClick();
        _settingsMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        AudioManager.instance.PlayUIClick();
        _settingsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayUIClick();
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
