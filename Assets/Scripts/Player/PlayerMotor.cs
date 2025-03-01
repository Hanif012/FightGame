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

    AudioManager audioManager;
    private bool isPlayingRunSound = false;
    private bool isPlayingWalkSound = false;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
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

        if (sPlayer.diGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking || sPlayer.isUlti )
        {
            if (!isGrounded)
            {
                sPlayer.isBlocking = false;
            }
            return;
        }

        Vector2 moveDirection = new Vector2(input.x, 0f);

        // Mengecek kecepatan berdasarkan input horizontal
        if (input.x == 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            StopMovementSounds();
            return;

        }
        else if (input.x > -0.5f && input.x < 0.5f)  // Rentang nilai antara -0.5 dan 0.5
        {
            speed = 2;
            if (!isPlayingWalkSound)
            {
                StopMovementSounds();
                audioManager.PlaySFX(audioManager.walk);
                isPlayingWalkSound = true;
                isPlayingRunSound = false;
            }

        }
        else if (input.x == 1f || input.x == -1f) // Kecepatan tinggi untuk nilai input tertentu
        {
            speed = 5;
            if (!isPlayingRunSound)
            {
                StopMovementSounds();
                audioManager.PlaySFX(audioManager.run);
                isPlayingRunSound = true;
                isPlayingWalkSound = false;
            }

        }
        rb.linearVelocity = new Vector2(moveDirection.normalized.x * speed, rb.linearVelocity.y);
        sPlayer.animator.SetBool("isJumping", !isGrounded);
        sPlayer.animator.SetFloat("xVelocity", Mathf.Abs(input.x));
        sPlayer.animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }


    // Fungsi loncat
    public void Jump()
    {
        if (sPlayer.diGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking)
        {
            return;
        }
        Debug.Log("Input Jump");

        if (isGrounded)
        {
            doubleJump = true;
            // Menambahkan gaya loncatan
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));
            audioManager.PlaySFX(audioManager.jump);

        }
        else if (doubleJump)
        {
            doubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));
            audioManager.PlaySFX(audioManager.jump);

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

    private void StopMovementSounds()
    {
        audioManager.StopSFX();
        isPlayingWalkSound = false;
        isPlayingRunSound = false;
    }
}