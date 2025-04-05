using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 input = Vector2.zero; // Input vector for movement
    public float moveSpeed = 5f; // Movement speed of the player
    public float jumpForce = 10f; // Jump force

    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck; // Position to check if the player is grounded
    public LayerMask groundLayer; // Ground layer mask

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(transform.localScale.x * 0.9f, 0.1f), 0, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
