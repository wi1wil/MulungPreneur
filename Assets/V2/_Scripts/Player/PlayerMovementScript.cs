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
        _movement = context.ReadValue<Vector2>();

        if (context.performed)
        {
            _anim.SetBool("isWalking", true);
        }

        if (context.canceled)
        {
            _anim.SetBool("isWalking", false);
            _anim.SetFloat("LastInputX", _movement.x);
            _anim.SetFloat("LastInputY", _movement.y);
            _walkTimer = 0f; // reset timer when stop
        }

        _anim.SetFloat("InputX", _movement.x);
        _anim.SetFloat("InputY", _movement.y);
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
