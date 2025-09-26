using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private Vector2 movement;
    // private AudioManager audio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        // audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        anim.SetBool("isWalking", true);
        if (context.canceled)
        {
            anim.SetBool("isWalking", false);
            anim.SetFloat("LastInputX", movement.x);
            anim.SetFloat("LastInputY", movement.y);
        }

        movement = context.ReadValue<Vector2>();
        anim.SetFloat("InputX", movement.x);
        anim.SetFloat("InputY", movement.y);
    }

    void FixedUpdate()
    {
        if (MenuPauseManager.instance.gamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement * speed;
    }
}
