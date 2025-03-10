using UnityEngine;
using UnityEngine.UI;

public class UltChargeUI : MonoBehaviour
{
    [SerializeField] private Slider ultSlider;
    public Slider UltSlider => ultSlider; 

    private float ultChargeTime;
    private float currentCharge;
    private bool isCharging; 

    public void InitializeUltCharge(float chargeTime)
    {
        ultChargeTime = chargeTime;
        currentCharge = 0;

        if (ultSlider != null)
        {
            ultSlider.maxValue = ultChargeTime;
            ultSlider.value = 0;
        }
    }

    public void StartCharging()
    {
        isCharging = true; 
        currentCharge = 0;

        if (ultSlider != null)
            ultSlider.value = 0;

        Debug.Log("UltChargeUI: StartCharging() CALLED - isCharging set to TRUE.");
    }

    public void ResetUltCharge()
    {
        currentCharge = 0;
        isCharging = true;  
        if (ultSlider != null)
            ultSlider.value = 0;
    }

    private void Update()
    {
        if (!isCharging) return; 

        currentCharge += Time.deltaTime;
        if (ultSlider != null)
            ultSlider.value = currentCharge;

        if (currentCharge >= ultChargeTime)
        {
            isCharging = false;  
            Debug.Log(" UltChargeUI: Ult Fully Charged!");
        }
    }

    public bool IsUltReady()
    {
        return currentCharge >= ultChargeTime;
    }
}
