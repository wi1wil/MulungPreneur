using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    public float movementSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public AudioManagerScript audioScript;
    readonly float sfxInterval = 0.6f;
    float intervalTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioScript = FindObjectOfType<AudioManagerScript>();
        if (!audioScript)
        audioScript = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }

    void Update()
    {
        if (PauseControllerScript.isGamePaused)
        {
            rb.velocity = Vector2.zero; // Stop movement when the game is paused
            return;
        }
        rb.velocity = moveInput * movementSpeed;

        WalkSFX();

        animator.SetBool("isWalking", rb.velocity.magnitude > 0);
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if(context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    void WalkSFX()
    {
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            intervalTimer += Time.deltaTime;
            if (intervalTimer > sfxInterval)
            {
                intervalTimer = 0f;
                audioScript.PlaySfx(audioScript.walkDefault);
            }
        }
        else
        {
            intervalTimer = 1f;
        }
    }
}
