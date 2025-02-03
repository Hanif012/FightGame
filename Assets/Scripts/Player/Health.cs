using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    private float currentHealth;
    [SerializeField] private Transform respawnPoint; // Unique respawn point for each player

    private void Start()
    {
        currentHealth = maxHealth;
        LivesManager.Instance.RegisterPlayer(this, respawnPoint); // Register the player with LivesManager
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            LivesManager.Instance.LoseLife(this);
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
