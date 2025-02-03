using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the kill zone has a Health component
        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth != null)
        {
            Debug.Log(other.gameObject.name + " fell off the platform!");

            // Simulate losing all health when falling
            playerHealth.TakeDamage(playerHealth.GetCurrentHealth());
        }
    }
}

