using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject pauseMenuUI;

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MenuPauseManager.instance.TogglePause();

            if (MenuPauseManager.instance.gamePaused)
            {
                playerInput.SwitchCurrentActionMap("UI");
                if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
            }
            else
            {
                playerInput.SwitchCurrentActionMap("Player");
                if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
            }
        }
    }
}