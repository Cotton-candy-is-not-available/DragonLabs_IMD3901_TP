using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BatChargeHits : MonoBehaviour
{
    private float indicatorTimer = 1.0f;
    private float maxIndicatorTimer = 1.0f;
    [SerializeField] private Image hitChargeCircle;
    KeyCode selectKey = KeyCode.Mouse0; //left mouse click for PC
    private bool shouldUpdate = false;

    public float hitForce = 0.0f;
    public float maxHitForce = 100.0f;
    public PiñataController piñataController_access;


    private void Update()
    {
        if (Input.GetKey(selectKey)) //if LMB is being held down
        {
            indicatorTimer -= Time.deltaTime;
            indicatorTimer = Mathf.Max(indicatorTimer, 0f);
            hitChargeCircle.enabled = true;
            hitChargeCircle.fillAmount = indicatorTimer / maxIndicatorTimer;
        }
        else //if the LMB is NOT being held down anymore
        {
            if (shouldUpdate == true)
            {
                indicatorTimer += Time.deltaTime;
                
                if (indicatorTimer >= maxIndicatorTimer)
                {
                    indicatorTimer = maxIndicatorTimer;
                    hitChargeCircle.enabled = false;
                    shouldUpdate = false;
                }
                hitChargeCircle.fillAmount = indicatorTimer / maxIndicatorTimer;
            }
        }

        //for looping its charge again
        if (Input.GetKey(selectKey))
        {
            shouldUpdate = true;

            float normalizedCharge = 1f - (indicatorTimer / maxIndicatorTimer);
            normalizedCharge = Mathf.Clamp01(normalizedCharge);
            hitForce = normalizedCharge * maxHitForce;
        }
        if (Input.GetKeyUp(selectKey)) //apply the hit force when LMB is released
        {
            piñataController_access.applyHitChargeForce(hitForce);
        }
    }
}
