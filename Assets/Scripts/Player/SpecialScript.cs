using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSpecial : MonoBehaviour
{
    [Header("Special Attack Settings")]
    [SerializeField] private Transform specialBoxOrigin; // Origin of the special attack box
    [SerializeField] private Vector2 specialBoxSize = new Vector2(1.0f, 1.0f); // Size of the attack box
    [SerializeField] private LayerMask targetLayer;     // Layer to detect targets
    [SerializeField] private float attackCooldown = 1f; // Cooldown between attacks
    [SerializeField] private int attackDamage = 2;       // Damage dealt per attack

    private float lastAttackTime = 0;
    private PlayerCondition sPlayer;

    private List<PlayerCondition> hitEnemiesList = new List<PlayerCondition>(); // Menyimpan semua musuh yang terkena hit
    AudioManager audioManager;
    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        sPlayer = GetComponent<PlayerCondition>();

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) 
        {
            PerformSpecial();
        }
    }

    public void PerformSpecial()
    {
        // Cooldown check
        if (Time.time < lastAttackTime + attackCooldown)
            return;
        if (sPlayer.diGrab || sPlayer.ngeGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking || sPlayer.isAttack || sPlayer.isUlti)
            return;
        lastAttackTime = Time.time;
        Debug.Log("Player is special attacking!");
        audioManager.PlaySFX(audioManager.attack);

        sPlayer.FalseAllAnimation();
        sPlayer.specialAttacking = true;
        sPlayer.animator.SetBool("isSpecialAtk", sPlayer.specialAttacking);


        // Detect targets in the attack box
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(specialBoxOrigin.position, specialBoxSize, 0f, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            if (target.gameObject == gameObject)continue;
            Transform enemy = target.transform;
            Debug.Log($"Hit: {target.name}");
            PlayerCondition sEmyPlayer = target.GetComponent<PlayerCondition>();


            // Apply damage to the target's Health component
            Health health = target.GetComponent<Health>();
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
                health.TakeDamage(attackDamage);

                sEmyPlayer.FalseAllAnimation();
                sEmyPlayer.isHurt = true;
                sEmyPlayer.animator.SetBool("isHurt", sEmyPlayer.isHurt);
                hitEnemiesList.Add(sEmyPlayer);
            }
        }
        Debug.Log("Player is special attacking!2");

        Invoke(nameof(ResetHurt), 1);
        Invoke(nameof(ResetAttack), 0.5f);
        Debug.Log("Player is special attacking!3");

    }

    private void ResetAttack()
    {
        sPlayer.specialAttacking = false;
        sPlayer.animator.SetBool("isSpecialAtk", sPlayer.specialAttacking);
    }

     void ResetHurt()
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
        // Visualize the attack box in the Scene view
        if (specialBoxOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(specialBoxOrigin.position, specialBoxSize);
    }
}
