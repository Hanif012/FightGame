using NUnit.Framework;
using UnityEngine;
using UnityEngine.Windows;

public class MovementEnemyScript : MonoBehaviour
{
    public GameObject allPlayer;
    public float targetDistance = 4;
    public float stopDistance = 2;
    public float moveSpeed = 5f;
    public float jumpHeight = 2;
    public float gravity = -9.81f;
    public float patrolTime = 3f; // Waktu patroli sebelum ganti arah
    public float grabbingTime = 5;

    private Rigidbody2D rb;
    private float lastGrabTime = 0;
    private bool doubleJump = false;
    private bool isPatrolling = false;
    private float patrolTimer = 0f;
    private int patrolDirection = 1; // 1 = kanan, -1 = kiri

    [SerializeField] private Transform platformGroundCheck;
    [SerializeField] private Transform characterGroundCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    private bool isShouldJump;
    private bool isShouldRotate;
    private bool isOkeyToFall;

    private PlayerCondition sPlayer;
    private PlayerAttack playerAttack;
    private BlockScript blockScript;
    private PlayerGrabnThrow playerGrabnThrow;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
        blockScript = GetComponent<BlockScript>();
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        isShouldJump = IsShouldJump();
        isShouldRotate = IsShouldRotate();
        isOkeyToFall = IsOkeyToFall();

        GameObject nearestPlayer = null;
        float distance = Mathf.Infinity;
        Vector3 ePosition = Vector3.zero;
        Vector3 direction = Vector3.zero;
        PlayerCondition eCondition = null;

        for (int i = 0; i < allPlayer.transform.childCount; i++)
        {
            GameObject playerGameObject = allPlayer.transform.GetChild(i).gameObject;
            if (!playerGameObject.activeSelf || playerGameObject == gameObject)
                continue;
            Vector3 distanceVector = (playerGameObject.transform.position - transform.position);
            if (nearestPlayer == null || distanceVector.magnitude < distance)
            {
                nearestPlayer = playerGameObject;
                distance = distanceVector.magnitude;
                direction = distanceVector.normalized;
                ePosition = playerGameObject.transform.position;
                eCondition = nearestPlayer.GetComponent<PlayerCondition>();

            }
        }

        if (nearestPlayer != null && distance < targetDistance && distance > stopDistance && sPlayer.ngeGrab)
        {
            isPatrolling = false; // Hentikan patroli

            if (sPlayer != null && (sPlayer.diGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking ))
                return;

            // Mengubah arah hadap karakter
            transform.rotation = direction.x < 0f ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

            // AI lompat hanya kalau di ujung platform DAN di tanah
            if (isShouldJump && isGrounded)
            {
                doubleJump = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));
            }
            else if (transform.position.y <= ePosition.y && !isGrounded && doubleJump)
            {
                doubleJump = false;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));
            }
        }
        else if (distance < stopDistance + (stopDistance / 2))
        {

            if (eCondition.isAttack )
            {
                blockScript.PerformDeffend(true);

            }else if (Time.time < lastGrabTime + grabbingTime )
            {
                playerGrabnThrow.PerformGrabnThrow();
                lastGrabTime = Time.time;
            }
            else
            {
                blockScript.PerformDeffend(false);
                playerAttack.PerformAttack();
                transform.rotation = direction.x < 0f ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            }

        }
        else
        {

            if (distance < stopDistance + (stopDistance / 2))
            {
                return;
            }

            if (sPlayer != null && (sPlayer.diGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking))
                return;
            // **Mode Patroli**
            if (!isPatrolling)
            {
                isPatrolling = true;
                patrolTimer = patrolTime; // Reset timer
                patrolDirection = Random.Range(0, 2) == 0 ? -1 : 1; // Pilih arah awal secara acak

                patrolTimer -= Time.deltaTime;
                if (patrolTimer <= 0)
                {
                    patrolDirection *= -1; // Ganti arah setelah beberapa detik
                    patrolTimer = patrolTime;
                }

                // Gerakan patroli
                transform.rotation = patrolDirection == 1 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
            }
            rb.linearVelocity = new Vector2(patrolDirection * moveSpeed, rb.linearVelocity.y);

            // AI lompat jika di ujung platform
            if (isShouldJump && isGrounded)
            {
                if (isShouldRotate || !isOkeyToFall)
                {
                    float newRotationY = transform.rotation.eulerAngles.y == 0 ? 180 : 0;
                    transform.rotation = Quaternion.Euler(0, newRotationY, 0);
                    patrolDirection *= -1;

                }

                else if (!isShouldRotate && isGrounded)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity));

                }
            }
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(characterGroundCheck.position, 1f, groundLayer);
    }

    private bool IsShouldJump()
    {
        return !Physics2D.OverlapCircle(platformGroundCheck.position, 1f, groundLayer);
    }
    private bool IsShouldRotate()
    {

        return !Physics2D.Raycast(platformGroundCheck.position,new Vector3(1,0,0), 2, groundLayer);
    }
    private bool IsOkeyToFall()
    {
        return Physics2D.Raycast(platformGroundCheck.position, new Vector3(0, -1, 0), 2, groundLayer);
    }


}
