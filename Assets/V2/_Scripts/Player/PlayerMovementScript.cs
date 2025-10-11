using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sprite;
    private Vector2 _movement;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _anim.SetBool("isWalking", true);
        if (context.canceled)
        {
            _anim.SetBool("isWalking", false);
            _anim.SetFloat("LastInputX", _movement.x);
            _anim.SetFloat("LastInputY", _movement.y);
        }

        _movement = context.ReadValue<Vector2>();
        _anim.SetFloat("InputX", _movement.x);
        _anim.SetFloat("InputY", _movement.y);
    }

    void FixedUpdate()
    {
        if (MenuPauseManager.instance.gamePaused)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        _rb.linearVelocity = _movement * speed;
    }
}
