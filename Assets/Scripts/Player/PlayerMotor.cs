using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float speed = 8f;
    public float jumpHeight = 16f;
    public float gravity = -9.81f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private bool isGrounded;

    private bool doubleJump;
    protected PlayerCondition sPlayer;


    private InputManager inputManager;
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        sPlayer = GetComponent<PlayerCondition>();
    }

    void Update()
    {

        isGrounded = IsGrounded();
        sPlayer.isJumping = !isGrounded;
    }

    // Gerak
    public void ProcessMove(Vector2 input)
    {


        // Mengubah arah hadap karakter
        if (input.x < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (input.x > 0f)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }

        if (sPlayer.diGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking)
        {

            return;
        }

        Vector2 moveDirection = new Vector2(input.x, 0f);

        // Mengecek kecepatan berdasarkan input horizontal
        if (input.x == 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        }
        else if (input.x > -0.5f && input.x < 0.5f)  // Rentang nilai antara -0.5 dan 0.5
        {
            speed = 2;
        }
        else if (input.x == 1f || input.x == -1f) // Kecepatan tinggi untuk nilai input tertentu
        {
            speed = 5;
        }
        rb.linearVelocity = new Vector2(moveDirection.normalized.x * speed, rb.linearVelocity.y);
        sPlayer.animator.SetBool("isJumping", !isGrounded);
        sPlayer.animator.SetFloat("xVelocity", Mathf.Abs(input.x));
        sPlayer.animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }


    // Fungsi loncat
    public void Jump()
    {
        if (sPlayer.diGrab)
        {
            return;
        }
        Debug.Log("Input Jump");

        if (isGrounded)
        {
            doubleJump = true;
            // Menambahkan gaya loncatan
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));

        }
        else if (doubleJump)
        {
            doubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));

        }
        sPlayer.animator.SetBool("isJumping", !isGrounded);

    }

    // Cek apakah karakter berada di tanah
    private bool IsGrounded()
    {

        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Test");
        sPlayer.animator.SetBool("isJumping", !isGrounded);

    }
}