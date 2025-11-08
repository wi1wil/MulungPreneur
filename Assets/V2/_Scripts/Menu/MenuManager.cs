using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _pauseMenuUI;

    [SerializeField] private GameObject[] _otherMenu;

    private void Start()
    {
        _pauseMenuUI?.SetActive(false);
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (CheckOpenedMenu()) return;

        bool shouldPause = !MenuPauseManager.instance.gamePaused;
        MenuPauseManager.instance.SetPaused(shouldPause);
        AudioManager.instance.PlayUIClick();

        if (shouldPause)
        {
            _playerInput.SwitchCurrentActionMap("UI");
            if (_pauseMenuUI != null) _pauseMenuUI.SetActive(true);
        }
        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
            if (_pauseMenuUI != null) _pauseMenuUI.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        MenuPauseManager.instance.SetPaused(false);
        AudioManager.instance.PlayUIClick();
        _playerInput.SwitchCurrentActionMap("Player");
        if (_pauseMenuUI != null) _pauseMenuUI.SetActive(false);
    }
    
    public bool CheckOpenedMenu()
    {
        for(int i = 0; i < _otherMenu.Length; i++)
        {
            if (_otherMenu[i].activeSelf)
            {
                Debug.Log($"Menu {i} is opened.");
                return true;
            }
        }
        return false;
    }
}