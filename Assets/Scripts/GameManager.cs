using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnCharacterSelect;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartGame()
    {
        Debug.Log("Game Started");
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        Debug.Log("Game Over");
        OnGameOver?.Invoke();
    }

    public void CompleteLevel()
    {
        Debug.Log("Select Character");
        OnCharacterSelect?.Invoke();
    }
}
