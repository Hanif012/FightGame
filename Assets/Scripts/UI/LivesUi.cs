using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> heartImages; 
    private int maxLives;

    private void Start()
    {
        maxLives = heartImages.Count; 
    }

    public void UpdateLivesUI(int lives)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].SetActive(i < lives); 
        }
    }
}
