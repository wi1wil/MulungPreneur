using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _pauseMenuUI;

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MenuPauseManager.instance.TogglePause();

            if (MenuPauseManager.instance.gamePaused)
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
    }
}