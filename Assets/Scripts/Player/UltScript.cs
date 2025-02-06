using UnityEngine;

public class PlayerUlt : MonoBehaviour
{
    [Header("Ult Settings")]
    [SerializeField] private Transform ultAttackOrigin; // Position where ult happens
    [SerializeField] private Vector2 ultAttackSize = new Vector2(3.0f, 2.0f); // Area of the ult
    [SerializeField] private LayerMask targetLayer; // What the ult can hit
    [SerializeField] private float ultCooldown = 10f; // Wait time before using ult again
    [SerializeField] private int ultDamage = 50; // Big damage

    private float lastUltTime = -10f; // Track last ult usage
    AudioManager audioManager;
    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            PerformUlt();
        }
    }

    public void PerformUlt()
    {
        // Check if enough time has passed since the last ult
        if (Time.time < lastUltTime + ultCooldown)
        {
            Debug.Log("Ult is on cooldown!");
            return;
        }

        lastUltTime = Time.time; 
        Debug.Log("Ultimate");
        audioManager.PlaySFX(audioManager.attack);

        // Detect targets in the ult range
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(ultAttackOrigin.position, ultAttackSize, 0f, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            if (target.gameObject == gameObject)continue;

            Debug.Log($"ULT hit: {target.name}");

            // Apply high damage to the target if they have a Health component
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(ultDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ult attack area in the Scene view
        if (ultAttackOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(ultAttackOrigin.position, ultAttackSize);
    }
}

