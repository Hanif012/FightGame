using System.Collections.Generic;
using UnityEngine;

public class PlayerUlt : MonoBehaviour
{
    [SerializeField] private Transform ultAttackOrigin;
    [SerializeField] private Vector2 ultAttackSize = new Vector2(3.0f, 2.0f);
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] public float ultCooldown = 10f;
    [SerializeField] private int ultDamage = 50;
    [SerializeField] private UltChargeUI ultChargeUI;

    private bool isUltReady = false;
    private AudioManager audioManager;
    private PlayerCondition sPlayer;
    private List<PlayerCondition> hitEnemiesList = new List<PlayerCondition>();

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void Start()
    {
        sPlayer = GetComponent<PlayerCondition>();

        if (ultChargeUI == null)
        {
            ultChargeUI = Object.FindFirstObjectByType<UltChargeUI>();
            if (ultChargeUI == null)
            {
                Debug.LogError("⚠️ PlayerUlt: UltChargeUI is missing! Assign it manually in Unity.");
                return;
            }
        }

        ultChargeUI.InitializeUltCharge(ultCooldown);
        ultChargeUI.StartCharging();
    }

    private void Update()
    {
        if (ultChargeUI != null)
        {
            isUltReady = ultChargeUI.IsUltReady();
        }

        if (Input.GetKeyDown(KeyCode.E) && isUltReady)
        {
            PerformUlt();
        }
    }

    public void PerformUlt()
    {
        if (!isUltReady) return; // ✅ Ensures ult is only used when charged

        if (sPlayer.diGrab || sPlayer.ngeGrab || sPlayer.isBlocking || sPlayer.isKnock || sPlayer.specialAttacking || sPlayer.isUlti)
            return;

        if (audioManager != null) 
            audioManager.PlaySFX(audioManager.attack);

        sPlayer.FalseAllAnimation();
        sPlayer.isUlti = true;
        sPlayer.animator.SetBool("isUlt", true);

        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(ultAttackOrigin.position, ultAttackSize, 0f, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            if (target.gameObject == gameObject) continue;

            Health health = target.GetComponent<Health>();
            PlayerCondition enemyPlayer = target.GetComponent<PlayerCondition>();

            if (health != null)
            {
                health.TakeDamage(ultDamage);

                if (enemyPlayer != null)
                {
                    enemyPlayer.FalseAllAnimation();
                    enemyPlayer.isHurt = true;
                    enemyPlayer.animator.SetBool("isHurt", true);
                    hitEnemiesList.Add(enemyPlayer);
                }
            }
        }

        // ✅ Ensure ult recharge starts properly
        if (ultChargeUI != null)
        {
            ultChargeUI.ResetUltCharge();
            ultChargeUI.StartCharging();
        }

        if (hitEnemiesList.Count > 0)
            Invoke(nameof(ResetHurt), 1f);

        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void ResetAttack()
    {
        sPlayer.isUlti = false;
        sPlayer.animator.SetBool("isUlt", false);
    }

    private void ResetHurt()
    {
        foreach (PlayerCondition enemy in hitEnemiesList)
        {
            if (enemy != null)
            {
                enemy.isHurt = false;
                enemy.animator.SetBool("isHurt", false);
            }
        }
        hitEnemiesList.Clear();
    }
}
