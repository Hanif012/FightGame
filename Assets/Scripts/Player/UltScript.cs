using UnityEngine;

public class PlayerUlt : MonoBehaviour
{
    [Header("Ult Settings")]
    [SerializeField] private Transform ultAttackOrigin; // Position where ult happens
    [SerializeField] private Vector2 ultAttackSize = new Vector2(3.0f, 2.0f); // Area of the ult
    [SerializeField] private LayerMask targetLayer; // What the ult can hit
    [SerializeField] private float ultCooldown = 10f; // Time needed to fully charge
    [SerializeField] private int ultDamage = 50; // Big damage
    [SerializeField] private UltChargeUI ultChargeUI; // Reference to Ult Charge UI

    private float lastUltTime = -10f; // Track last ult usage
    private bool isUltReady = false;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        
        if (ultChargeUI != null)
        {
            ultChargeUI.InitializeUltCharge(ultCooldown);
            ultChargeUI.StartCharging();
        }
    }

    private void Update()
    {
        
        if (ultChargeUI != null)
        {
            isUltReady = ultChargeUI.IsUltReady();
        }

        
        if (Input.GetKeyDown(KeyCode.E) && isUltReady)
        {
            PerformUlt();
        }
    }

    public void PerformUlt()
    {
        if (!isUltReady) return; 

        lastUltTime = Time.time;
        Debug.Log("Ultimate Activated!");
        audioManager.PlaySFX(audioManager.attack);

        // Detect targets in the ult range
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(ultAttackOrigin.position, ultAttackSize, 0f, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            if (target.gameObject == gameObject) continue; 

            Debug.Log($"ULT hit: {target.name}");

            // Apply high damage to the target if they have a Health component
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(ultDamage);
            }
        }

     
        if (ultChargeUI != null)
        {
            ultChargeUI.ResetUltCharge();
            ultChargeUI.StartCharging();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ult attack area in the Scene view
        if (ultAttackOrigin == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(ultAttackOrigin.position, ultAttackSize);
    }
}
