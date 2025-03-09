using UnityEngine;
using System.Collections.Generic;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance { get; private set; }

    [SerializeField] private int maxLives = 3;
    private Dictionary<Health, int> playerLives = new Dictionary<Health, int>(); 
    private Dictionary<Health, Transform> respawnPoints = new Dictionary<Health, Transform>(); 
    private Dictionary<Health, PlayerUI> playerUIs = new Dictionary<Health, PlayerUI>(); 

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

    // ✅ FIXED: Now correctly expects a respawnPoint
    public void RegisterPlayer(Health player, Transform respawnPoint, PlayerUI playerUI, float ultChargeTime)
    {
        if (!playerLives.ContainsKey(player))
        {
            playerLives[player] = maxLives;
            respawnPoints[player] = respawnPoint;
            playerUIs[player] = playerUI; 

            // ✅ FIXED: Get max health correctly
            playerUI.SetupUI(player.CharacterFrame, Mathf.RoundToInt(player.GetMaxHealth()), maxLives, ultChargeTime);

            Debug.Log($"{player.CharacterFrame.name} registered with respawn point at {respawnPoint.position}");
        }
        else
        {
            Debug.LogWarning($"{player.CharacterFrame.name} is already registered!");
        }
    }

    public void LoseLife(Health player)
    {
        if (!playerLives.ContainsKey(player)) return; 

        playerLives[player]--;

        if (playerUIs.ContainsKey(player))
        {
            playerUIs[player].UpdateLives(playerLives[player]); 
        }

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
        if (!respawnPoints.ContainsKey(player))
        {
            Debug.LogError($"No respawn point found for {player.CharacterFrame.name}!");
            return;
        }

        Debug.Log($"{player.CharacterFrame.name} lost a life. Respawning...");

        player.ResetHealth();
        player.transform.position = respawnPoints[player].position;
    }

    private void GameOver(Health player)
    {
        Debug.Log($"{player.CharacterFrame.name} is out of lives!");
        player.gameObject.SetActive(false); 
    }

    public int GetCurrentLives(Health player)
    {
        return playerLives.ContainsKey(player) ? playerLives[player] : 0;
    }
}
