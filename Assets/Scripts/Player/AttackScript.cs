using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform attackBoxOrigin;
    [SerializeField] private Vector2 attackBoxSize = new Vector2(1.0f, 1.0f);
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float attackCooldown = 0.2f;
    [SerializeField] private int attackDamage = 1;

    private float lastAttackTime = 0;
    private InputManager inputManager;
    private PlayerCondition sPlayer;
    private PlayerCondition sEmyPlayer;
    private List<PlayerCondition> hitEnemiesList = new List<PlayerCondition>(); // Menyimpan semua musuh yang terkena hit
    AudioManager audioManager;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        sPlayer = GetComponent<PlayerCondition>();

        if (sPlayer == null)
        {
            Debug.LogError("PlayerCondition tidak ditemukan di " + gameObject.name);
        }
    }

    public void PerformAttack()
    {
        if (sPlayer == null) return;

        if (sPlayer.diGrab || sPlayer.ngeGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking || sPlayer.isUlti)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;
        lastAttackTime = Time.time;

        audioManager.PlaySFX(audioManager.attack);


        sPlayer.FalseAllAnimation();
        sPlayer.isAttack = true;
        sPlayer.animator.SetBool("isAttacking", sPlayer.isAttack);

        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(attackBoxOrigin.position, attackBoxSize, 0f, targetLayer);
        Debug.Log(hitTargets.Length);
        foreach (Collider2D target in hitTargets)
        {
            if (target.gameObject == gameObject) continue;
            Transform enemy = target.transform;
            Debug.Log($"Hit: {target.name}");

            Health health = target.GetComponent<Health>();
            sEmyPlayer = target.GetComponent<PlayerCondition>();

            if (health != null)
            {
                if (sEmyPlayer != null && sEmyPlayer.isBlocking)
                {
                    Vector3 directionToTarget = (enemy.position - transform.position).normalized;
                    Vector3 enemyLook = enemy.transform.forward;
                    float dot = Vector3.Dot(directionToTarget, enemyLook);
                    if (dot < 0f || sEmyPlayer.isHurt || sEmyPlayer.isKnock)
                    {
                        sEmyPlayer = null;
                        continue;
                    }
                }
                Debug.Log("hit!");
                health.TakeDamage(attackDamage);
                sEmyPlayer.FalseAllAnimation();
                sEmyPlayer.isHurt = true;
                sEmyPlayer.animator.SetBool("isHurt", sEmyPlayer.isHurt);
                hitEnemiesList.Add(sEmyPlayer);
            }

        }
        Invoke(nameof(ResetHurt), 1);
        Invoke(nameof(ResetAttack), 0.5f);

    }

    private void ResetAttack()
    {
        sPlayer.isAttack = false;
        sPlayer.animator.SetBool("isAttacking", sPlayer.isAttack);
    }
    private void ResetHurt()
    {
        foreach (PlayerCondition sEmyPlayerr in hitEnemiesList)
        {
            sEmyPlayerr.isHurt = false;
            sEmyPlayerr.animator.SetBool("isHurt", sEmyPlayerr.isHurt);

        }
        hitEnemiesList.Clear();


    }
    private void OnDrawGizmosSelected()
    {
        if (attackBoxOrigin == null) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(attackBoxOrigin.position, attackBoxSize);
    }
}