/*using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 8f;
    public float jumpForce = 12f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask Ground;

    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, Ground);

       
        float xInput = Input.GetAxis("Horizontal");
        rb2D.velocity = new Vector2(xInput * speed, rb2D.velocity.y);

        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }
    }

    public void StopMoving()
    {       
        rb2D.velocity = Vector2.zero;
        this.enabled = false;
    }
}
*/

using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    float horizontalInput;
    bool isFacingRight = true;

    Rigidbody2D rb;

    public static PlayerMovement Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Keyboard.current != null)
        {
            float moveLeft = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1 : 0;
            float moveRight = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1 : 0;
            horizontalInput = moveLeft + moveRight;
        }

        FlipSprite();
    }

    private void FixedUpdate()
    {

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }




}