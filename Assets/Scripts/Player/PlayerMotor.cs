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
    [SerializeField] private Transform character;

    private bool isGrounded;
    private bool doubleJump;

    private PlayerGrabnThrow playerGrabnThrow;
    private InputManager inputManager;

    void Start()
    {
        playerGrabnThrow = GetComponent<PlayerGrabnThrow>();
        inputManager = GetComponent<InputManager>();

    }

    void Update()
    {
        // Cek apakah karakter berada di tanah
        isGrounded = IsGrounded();

        // Input untuk loncat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    // Gerak
    public void ProcessMove(Vector2 input)
    {
        if (inputManager.diGrab)
            return;
        if (playerGrabnThrow.grabPlayer)
            return;
        Vector2 moveDirection = new Vector2(input.x, 0f);

        // Mengubah arah hadap karakter
        if (input.x < 0f)
        {
            character.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (input.x > 0f)
        {
            character.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Mengecek kecepatan berdasarkan input horizontal
        if (input.x == 0f)
        {
            speed = 0;
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
    }


    // Fungsi loncat
    public void Jump()
    {
        if (inputManager.diGrab)
            return;
        Debug.Log("Input Jump");

        if (isGrounded)
        {
            doubleJump = true;
            // Menambahkan gaya loncatan
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));
            Debug.Log("Loncat");
        }else if (doubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));
            doubleJump = false;
        }
    }

    // Cek apakah karakter berada di tanah
    private bool IsGrounded()
    {
        
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
