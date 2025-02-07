using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGrabnThrow : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private Transform playerFile;
    [Header("Hit Box")]
    [SerializeField] private Transform specialBoxOrigin; // Origin of the special attack box
    [SerializeField] private Vector2 specialBoxSize = new Vector2(1.0f, 1.0f); // Size of the attack box
    [Header("Grab")]
    [SerializeField] private Transform positionGrab;
    [SerializeField] private LayerMask targetLayer;     // Layer to detect targets
    [SerializeField] private float grabCooldown = 1f;   // Cooldown between grabs

    private Rigidbody2D rb;

    [Header("Grab Information")]
    private float lastGrabTime = 0;
    [SerializeField] private GameObject targetPlayer;
    private InputManager inputManager;

    private bool isGrabingFinal = false;

    [Header("Throw")]
    [SerializeField] float throwXPower = 10f;
    [SerializeField] float throwYPower = 5f;

    protected PlayerCondition sPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager = GetComponent<InputManager>();
        sPlayer = GetComponent<PlayerCondition>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // right mouse button for grab
        {
            PerformGrabnThrow();
        }

        // Ensure targetPlayer is following positionGrab only after it has been successfully grabbed
        if (targetPlayer != null && isGrabingFinal)
        {
            targetPlayer.transform.position = positionGrab.position;
        }
    }

    public void PerformGrabnThrow()
    {
        if (sPlayer.diGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking || sPlayer.isAttack || sPlayer.isHurt || sPlayer.specialAttacking)
            return;

        // Cooldown check
        if (Time.time < lastGrabTime + grabCooldown)
            return;

        rb.linearVelocity = Vector2.zero; // Stop the player's velocity temporarily

        // If already grabbing a player, release them
        if (!sPlayer.ngeGrab)
        {
            sPlayer.FalseAllAnimation();
            sPlayer.animator.SetBool("isGrabing", sPlayer.ngeGrab);

            Debug.Log("Releasing grabbed player");
            MegangMusuh();
            if (!sPlayer.ngeGrab)
            {
                sPlayer.ngeGrab = false;
                sPlayer.animator.SetBool("isGrabing", sPlayer.ngeGrab);
            }
            return;
        }

        Debug.Log("Attempting to grab player");
        LepasMusuh();
        sPlayer.ngeGrab = false;
        sPlayer.animator.SetBool("isGrabing", sPlayer.ngeGrab);
    }

    private void MegangMusuh()
    {
        lastGrabTime = Time.time;

        // Detect targets in the attack box
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(specialBoxOrigin.position, specialBoxSize, 0f, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            PlayerCondition sPlayerTarget = targetPlayer.GetComponent<PlayerCondition>();
            if (!sPlayerTarget.diGrab && !sPlayerTarget.ngeGrab)
            {
                sPlayerTarget.diGrab = true; // Set diGrab to true when grabbed
                                             // Ensure not grabbing the player itself
                if (target.gameObject == gameObject) continue;

                targetPlayer = target.gameObject; // Save the grabbed target
                targetPlayer.transform.parent = transform; // Parent the target to the player


                sPlayer.ngeGrab = true; // Mark that player is grabbed

                Rigidbody2D targetPlayerRb = targetPlayer.GetComponent<Rigidbody2D>();
                targetPlayerRb.linearVelocity = Vector2.zero; // Stop target's movement temporarily
                targetPlayerRb.gravityScale = 0f;


                isGrabingFinal = false;
                StartCoroutine(MoveToGrabPosition(targetPlayer));
                break; // Exit loop after grabbing one target                
            }

        }
    }

    private IEnumerator MoveToGrabPosition(GameObject target)
    {
        float duration = 0.5f; // Animation duration (adjust if needed)
        float elapsedTime = 0f;
        Vector3 startPos = target.transform.position;

        // Move the target smoothly to the grab position
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // Use SmoothStep for smoother easing
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            target.transform.position = Vector3.Lerp(startPos, positionGrab.position, smoothProgress);
            yield return null; // Wait for the next frame
        }

        // Ensure the target reaches the exact destination
        target.transform.position = positionGrab.position;
        isGrabingFinal = true; // Set isGrabingFinal to true when the target reaches the destination
    }

    private void LepasMusuh()
    {
        isGrabingFinal = false;

        targetPlayer.transform.parent = playerFile.transform;

        Rigidbody2D targetPlayerRb = targetPlayer.GetComponent<Rigidbody2D>();
        PlayerCondition sPlayerTarget = targetPlayer.GetComponent<PlayerCondition>();

        sPlayerTarget.isKnock = true;
        sPlayer.animator.SetBool("isKnock", sPlayer.isKnock);
        targetPlayerRb.gravityScale = 1f;
        if (transform.rotation.eulerAngles.y == 180)
        {
            // Pentalan ke kanan
            targetPlayerRb.linearVelocity = new Vector2(throwXPower, throwYPower);  // Kecepatan horizontal dan vertikal untuk efek pentalan
        }
        else if (transform.rotation.eulerAngles.y == 0)
        {
            // Pentalan ke kiri
            targetPlayerRb.linearVelocity = new Vector2(-(throwXPower), throwYPower);  // Kecepatan horizontal dan vertikal untuk efek pentalan
        }

        sPlayer.ngeGrab = false; // Reset grab status

        // Set diGrab on the target player to false (released)
        if (sPlayerTarget != null)
        {
            StartCoroutine(WaitAndRelease(sPlayerTarget));
        }


        targetPlayer = null; // Reset targetPlayer after release
    }

    private IEnumerator WaitAndRelease(PlayerCondition sPlayerTarget)
    {
        yield return new WaitForSeconds(2); // Wait for a moment before releasing
        if (sPlayerTarget != null)
        {
            sPlayerTarget.isKnock = false;
            sPlayer.animator.SetBool("isKnock", sPlayer.isKnock);
            sPlayerTarget.diGrab = false; // Set diGrab to false when released
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (specialBoxOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(specialBoxOrigin.position, specialBoxSize);
    }
}