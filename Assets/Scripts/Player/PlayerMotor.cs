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
    [SerializeField] private Animator animator;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJump ;
    private bool isPlayingWalkSound = false;
    private bool isPlayingRunSound = false;
    AudioManager audioManager;

    private bool doubleJump;

    private InputManager inputManager;

    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        isJump = !isGrounded;
        animator.SetBool("isJumping", !isGrounded);

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
        if (input.x < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (input.x > 0f)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }

        if (inputManager.diGrab)
        {
            return;
        }
        Vector2 moveDirection = new Vector2(input.x, 0f);

        // Mengubah arah hadap karakter


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
        animator.SetFloat("xVelocity", Mathf.Abs(moveDirection.normalized.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        HandleMovementSound(input.x);
    }


    // Fungsi loncat
    public void Jump()
    {
        if (inputManager.diGrab == true)
            return;
        
        if (isGrounded || doubleJump){
            Debug.Log("Input Jump");
            audioManager.PlaySFX(audioManager.jump);
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
            animator.SetBool("isJumping", !isGrounded);
        } 
    }

    // Manage movement sound
    private void HandleMovementSound(float horizontalInput){
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;
        bool isRunning = speed >= 5; 

        if (!isGrounded || !isMoving){
        StopMovementSounds();
        return;
    }
        
        if (isRunning){
            if (!isPlayingRunSound){
                StopMovementSounds(); // Stop previous sound before playing a new one
                audioManager.PlaySFX(audioManager.run);
                isPlayingRunSound = true;
            }
        }
        else{
            if (!isPlayingWalkSound){
                StopMovementSounds(); // Stop previous sound before playing a new one
                audioManager.PlaySFX(audioManager.walk);
                isPlayingWalkSound = true;
            }
        }
    }
    private void StopMovementSounds(){
        if (isPlayingWalkSound || isPlayingRunSound){
            audioManager.StopSFX();
        }
        isPlayingWalkSound = false;
        isPlayingRunSound = false;
    }


    // Cek apakah karakter berada di tanah
    private bool IsGrounded()
    {
        
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Test");
        animator.SetBool("isJumping", !isGrounded);

    }
}
