using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image characterFrame;
    public HealthBar healthBar;
    public UltChargeUI ultChargeUI;
    
    [Header("Lives System")]
    public List<Image> heartImages; 
    public Sprite fullHeartSprite;  
    public Sprite emptyHeartSprite; 

    private int maxLives;

    public void SetupUI(Sprite frame, int maxHealth, int maxLives, float ultChargeTime)
    {
        characterFrame.sprite = frame;
        healthBar.SetMaxHealth(maxHealth);
        ultChargeUI.InitializeUltCharge(ultChargeTime);

        this.maxLives = maxLives;
        UpdateLives(maxLives);
    }

    public void UpdateHealth(int health)
    {
        healthBar.SetHealth(health);
    }

    public void UpdateUltCharge(float charge)
    {
        if (ultChargeUI != null && ultChargeUI.UltSlider != null)
        {
            ultChargeUI.UltSlider.value = charge;  
        }
    }

    public void UpdateLives(int lives)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].sprite = (i < lives) ? fullHeartSprite : emptyHeartSprite;
        }
    }
}
