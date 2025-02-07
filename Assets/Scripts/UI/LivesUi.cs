using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> heartImages; // List of heart GameObjects

    public void UpdateLivesUI(int lives)
    {
        // Loop through all hearts
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < lives)
            {
                heartImages[i].SetActive(true); // Show heart
            }
            else
            {
                heartImages[i].SetActive(false); // Hide heart when life is lost
            }
        }
    }
}
