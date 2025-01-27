using UnityEngine;

public class PlayerSpecial : MonoBehaviour
{
    [Header("Special Attack Settings")]
    [SerializeField] private Transform specialBoxOrigin; // Origin of the special attack box
    [SerializeField] private Vector2 specialBoxSize = new Vector2(1.0f, 1.0f); // Size of the attack box
    [SerializeField] private LayerMask targetLayer;     // Layer to detect targets
    [SerializeField] private float attackCooldown = 1f; // Cooldown between attacks
    [SerializeField] private int attackDamage = 2;       // Damage dealt per attack

    private float lastAttackTime = 0;

    void Update()
    {
        // Detect input for attacking
        if (Input.GetMouseButtonDown(1)) // right mouse button for attack
        {
            PerformSpecial();
        }
    }

    public void PerformSpecial()
    {
        // Cooldown check
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Debug.Log("Player is special attacking!");

        // Detect targets in the attack box
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(specialBoxOrigin.position, specialBoxSize, 0f, targetLayer);

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
        if (specialBoxOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(specialBoxOrigin.position, specialBoxSize);
    }
}
