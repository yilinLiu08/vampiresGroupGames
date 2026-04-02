using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    [Header("Jump Settings")]
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;



    private Rigidbody2D rb;

    private Vector2 moveInput;

    private bool isSprinting;

    private bool jumpPressed;

    private bool isGrounded;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void Update()
    {
        CheckGround();

        UpdateAnimation();

        FlipSprite();
    }



    private void FixedUpdate()
    {
        MovePlayer();

        JumpPlayer();
    }



    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }



    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
        }
    }



    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }



    private void MovePlayer()
    {
        float currentSpeed = moveSpeed;

        if (isSprinting)
        {
            currentSpeed *= sprintMultiplier;
        }

        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
    }



    private void JumpPlayer()
    {
        if (!jumpPressed)
        {
            return;
        }

        if (!isGrounded)
        {
            jumpPressed = false;
            return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        jumpPressed = false;
    }



    private void CheckGround()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }



    private void FlipSprite()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (moveInput.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }



    private void UpdateAnimation()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetFloat("MoveX", Mathf.Abs(moveInput.x));
        animator.SetFloat("MoveYVelocity", rb.linearVelocity.y);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsRunning", Mathf.Abs(moveInput.x) > 0.01f);
        animator.SetBool("IsSprinting", isSprinting);
    }



    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}