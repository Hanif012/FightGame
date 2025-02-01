using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform attackBoxOrigin; // Origin of the attack box
    [SerializeField] private Vector2 attackBoxSize = new Vector2(1.0f, 1.0f); // Size of the attack box
    [SerializeField] private LayerMask targetLayer;     // Layer to detect targets
    [SerializeField] private float attackCooldown = 0.5f; // Cooldown between attacks
    [SerializeField] private int attackDamage = 1;       // Damage dealt per attack
    
    
    private float lastAttackTime = 0;
    private PlayerGrabnThrow playerGrabnThrow;
    private InputManager inputManager;

    private void Start()
    {
        playerGrabnThrow = GetComponent<PlayerGrabnThrow>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        // Detect input for attacking
        if (Input.GetMouseButtonDown(0)) // Left mouse button for attack
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        if (inputManager.diGrab)
            return;
        Debug.Log("test");
        // Cooldown check
        if (Time.time < lastAttackTime + attackCooldown)
            return;
        if (playerGrabnThrow.grabPlayer)
            return;

        lastAttackTime = Time.time;

        Debug.Log("Player is attacking!");

        // Detect targets in the attack box
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(attackBoxOrigin.position, attackBoxSize, 0f, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            if (target.gameObject == gameObject)continue;

            Debug.Log($"Hit: {target.name}");

            // Apply damage to the target's Health component
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack box in the Scene view
        if (attackBoxOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackBoxOrigin.position, attackBoxSize);
    }
}
