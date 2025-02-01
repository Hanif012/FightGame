using UnityEngine;

public class PlayerGrabnThrow : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private Transform playerFile;
    [SerializeField] private Transform specialBoxOrigin; // Origin of the special attack box
    [SerializeField] private Vector2 specialBoxSize = new Vector2(1.0f, 1.0f); // Size of the attack box
    [SerializeField] private LayerMask targetLayer;     // Layer to detect targets
    [SerializeField] private float grabCooldown = 1f;   // Cooldown between grabs
    [SerializeField] private Rigidbody2D rb;

    private float lastGrabTime = 0;
    [SerializeField] private GameObject targetPlayer;
    private InputManager inputManager;

    public bool grabPlayer = false;

    void Start()
    {
        inputManager = GetComponent<InputManager>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // right mouse button for grab
        {
            PerformGrabnThrow();
        }
    }

    public void PerformGrabnThrow()
    {
        if (inputManager.diGrab)
            return;

        // Cooldown check
        if (Time.time < lastGrabTime + grabCooldown)
            return;

        rb.linearVelocity = new Vector2(0, 0); // Stop the player�s velocity temporarily

        // Jika sudah memegang musuh, pindahkan dia ke `playerFile`
        if (grabPlayer && targetPlayer != null)
        {
            targetPlayer.transform.parent = playerFile.transform;

            Rigidbody2D targetPlayerRb = targetPlayer.GetComponent<Rigidbody2D>();

            // Menggunakan rotasi transform untuk menentukan arah pentalan
            if (transform.rotation.eulerAngles.y > 0)
            {
                // Pentalan ke kanan
                targetPlayerRb.linearVelocity = new Vector2(100, 5);  // Kecepatan horizontal dan vertikal untuk efek pentalan
            }
            else if (transform.rotation.eulerAngles.y == 0)
            {
                // Pentalan ke kiri
                targetPlayerRb.linearVelocity = new Vector2(-100, 5);  // Kecepatan horizontal dan vertikal untuk efek pentalan
            }

            grabPlayer = false; // Reset status grab

            // Set diGrab pada targetPlayer menjadi false (lepas)
            InputManager targetInputManager = targetPlayer.GetComponent<InputManager>();
            if (targetInputManager != null)
            {
                targetInputManager.diGrab = false; // Set diGrab to false when released
            }

            targetPlayer = null; // Reset targetPlayer setelah dilepas
            return;
        }

        lastGrabTime = Time.time;

        Debug.Log("Player is attempting to grab!");

        // Detect targets in the attack box
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(specialBoxOrigin.position, specialBoxSize, 0f, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            // Pastikan hanya menangkap musuh, bukan diri sendiri
            if (target.gameObject == gameObject) continue;

            targetPlayer = target.gameObject; // Simpan musuh yang tertangkap
            targetPlayer.transform.parent = specialBoxOrigin.transform; // Tempatkan di specialBoxOrigin
            grabPlayer = true; // Tandai bahwa musuh telah tertangkap

            Rigidbody2D targetPlayerRb = targetPlayer.GetComponent<Rigidbody2D>();
            targetPlayerRb.linearVelocity = Vector2.zero; // Stop target's movement temporarily

            // Set diGrab pada targetPlayer menjadi true (tergrab)
            InputManager targetInputManager = targetPlayer.GetComponent<InputManager>();
            if (targetInputManager != null)
            {
                targetInputManager.diGrab = true; // Set diGrab to true when grabbed
            }

            Debug.Log($"Grabbed {target.name}!");
            break; // Keluar dari loop setelah menangkap satu musuh
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (specialBoxOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(specialBoxOrigin.position, specialBoxSize);
    }
}
