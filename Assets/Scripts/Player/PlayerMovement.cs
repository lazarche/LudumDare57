using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    [Header("Movement Variables")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float gravityScale = 5f;
    [SerializeField] float fallGravityScale = 15f;
    [SerializeField] float fallingTimerMod = 5;

    Vector2 input = Vector2.zero;

    [Header("Internal Variables")]
    [SerializeField] bool isGrounded = false;
    [SerializeField] float fallingTimer = 0f;


    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), 0);
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);

        if (isGrounded)
        {
            rb.gravityScale = gravityScale;
            float jumpForce = Mathf.Sqrt((jumpHeight + fallingTimer * fallingTimerMod) * (Physics.gravity.y * rb.gravityScale) * -2f) * rb.mass;
            rb.AddForce(Vector2.up * (jumpForce ), ForceMode2D.Impulse);
            Debug.Log("Jumping");
        }

        if (rb.linearVelocity.y > 0)
        {
            rb.gravityScale = gravityScale;
            fallingTimer = 0f;
        }
        else
        {
            fallingTimer += Time.fixedDeltaTime;
            rb.gravityScale = fallGravityScale;
        }

    }

    void CheckGrounded()
    {
        Collider2D coillider = Physics2D.OverlapBox(groundCheck.position, new Vector2(transform.localScale.x * 0.9f, 0.1f), 0, groundLayer);
        if (coillider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(transform.localScale.x * 0.9f, 0.1f));
    }
}
