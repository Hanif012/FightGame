using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 5f;
    private float currentHealth;

    [Header("Character Info")]  
    public Sprite CharacterFrame; // Assign in Inspector (Character Portrait)

    [Header("UI References")]
    public HealthBar healthBar;
    public PlayerUI playerUI;

    [Header("Respawn Settings")]
    [SerializeField] private Transform respawnPoint; 

    private PlayerUlt playerUlt; 
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        playerUlt = GetComponent<PlayerUlt>(); // Get ult script
        if (playerUlt == null)
        {
            Debug.LogError($"PlayerUlt script missing on {gameObject.name}!");
            return;
        }

        // ✅ FIXED: Now passing `respawnPoint` to `RegisterPlayer()`
        LivesManager.Instance.RegisterPlayer(this, respawnPoint, playerUI, playerUlt.ultCooldown);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            LivesManager.Instance.LoseLife(this);
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
