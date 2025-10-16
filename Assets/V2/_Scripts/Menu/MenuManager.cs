using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _pauseMenuUI;

    private void Start()
    {
        _pauseMenuUI?.SetActive(false);
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (NPCDialogueManager.Instance.dialoguePanel.activeSelf || CrafterUIManagerV2.Instance.gameObject.activeSelf)
            return;
        
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
}