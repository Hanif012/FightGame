using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth != null)
        {
            Debug.Log(other.gameObject.name + " fell off the platform!");

            playerHealth.TakeDamage(playerHealth.GetCurrentHealth());
        }
    }
}

