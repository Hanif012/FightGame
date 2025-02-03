using UnityEngine;
using System.Collections.Generic; // Required for multiple players

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance { get; private set; }

    [SerializeField] private int maxLives = 3;
    private Dictionary<Health, int> playerLives = new Dictionary<Health, int>(); // Track lives per player
    private Dictionary<Health, Transform> respawnPoints = new Dictionary<Health, Transform>(); // Store respawn points

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Register players with their respawn points
    public void RegisterPlayer(Health player, Transform respawnPoint)
{
    if (!playerLives.ContainsKey(player))
    {
        playerLives[player] = maxLives;
        respawnPoints[player] = respawnPoint;

        Debug.Log(player.gameObject.name + " registered with respawn point at " + respawnPoint.position);
    }
    else
    {
        Debug.LogWarning(player.gameObject.name + " is already registered!");
    }
}


    public void LoseLife(Health player)
    {
        if (!playerLives.ContainsKey(player)) return; // Safety check

        playerLives[player]--;

        if (playerLives[player] > 0)
        {
            Respawn(player);
        }
        else
        {
            GameOver(player);
        }
    }

    private void Respawn(Health player)
    {
        Debug.Log(player.gameObject.name + " lost a life. Respawning...");

        player.ResetHealth();

        // Move the player back to their unique respawn point
        player.transform.position = respawnPoints[player].position;
    }

    private void GameOver(Health player)
    {
        Debug.Log(player.gameObject.name + " is out of lives!");
        player.gameObject.SetActive(false); 
    }

    public int GetCurrentLives(Health player)
    {
        return playerLives.ContainsKey(player) ? playerLives[player] : 0;
    }
}
