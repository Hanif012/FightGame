using UnityEngine;
using UnityEngine.UI;

public class UltBarUI : MonoBehaviour
{
    [SerializeField] private Slider ultSlider; // Assign in Inspector
    private float ultChargeTime; // Max time required to charge ult
    private float currentCharge; // Current ult charge

    private bool isCharging; // Whether ult is recharging

    public void InitializeUltCharge(float chargeTime)
    {
        ultChargeTime = chargeTime;
        currentCharge = 0;
        ultSlider.maxValue = ultChargeTime;
        ultSlider.value = 0;
        isCharging = false;
    }

    public void StartCharging()
    {
        isCharging = true;
        currentCharge = 0;
        ultSlider.value = 0;
    }

    private void Update()
    {
        if (isCharging && currentCharge < ultChargeTime)
        {
            currentCharge += Time.deltaTime;
            ultSlider.value = currentCharge;
        }
    }

    public bool IsUltReady()
    {
        return currentCharge >= ultChargeTime;
    }

    public void ResetUltCharge()
    {
        currentCharge = 0;
        ultSlider.value = 0;
        isCharging = false;
    }
}

