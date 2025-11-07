using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    private Rigidbody2D _rb;
    private Animator _anim;
    private Vector2 _movement;

    [Header("Footstep Settings")]
    [SerializeField] private float _walkSfxInterval = 0.6f;
    private float _walkTimer = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (context.performed)
        {
            _movement = input;

            // Update facing direction while moving
            if (_movement.sqrMagnitude > 0.01f)
            {
                _anim.SetFloat("InputX", _movement.x);
                _anim.SetFloat("InputY", _movement.y);
            }

            _anim.SetBool("isWalking", true);
        }
        else if (context.canceled)
        {
            // Only store last direction if the player was moving
            if (_movement.sqrMagnitude > 0.01f)
            {
                _anim.SetFloat("LastInputX", _movement.x);
                _anim.SetFloat("LastInputY", _movement.y);
            }

            _movement = Vector2.zero;
            _anim.SetBool("isWalking", false);
            _walkTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        if (MenuPauseManager.instance != null && MenuPauseManager.instance.gamePaused)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        _rb.linearVelocity = _movement * speed;

        HandleWalkSfx();
    }

    private void HandleWalkSfx()
    {
        if (_rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            _walkTimer += Time.deltaTime;
            if (_walkTimer >= _walkSfxInterval)
            {
                _walkTimer = 0f;
                AudioManager.instance.PlayWalkSFX();
            }
        }
        else
        {
            _walkTimer = _walkSfxInterval; 
        }
    }
}
