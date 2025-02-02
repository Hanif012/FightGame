using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject characterSelectScreen;
    [SerializeField] private GameObject gameOverScreen;
    private void Start()
    {
        GameManager.Instance.OnGameOver += ShowGameOverScreen;
        GameManager.Instance.OnCharacterSelect += ShowCharacterSelectScreen;
    }

    private void ShowGameOverScreen()
    {
        startScreen.SetActive(false);
        characterSelectScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    private void ShowCharacterSelectScreen()
    {
        startScreen.SetActive(false);
        characterSelectScreen.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= ShowGameOverScreen;
        GameManager.Instance.OnCharacterSelect -= ShowCharacterSelectScreen;
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void EndGame()
    {
        GameManager.Instance.EndGame();
    }

}
