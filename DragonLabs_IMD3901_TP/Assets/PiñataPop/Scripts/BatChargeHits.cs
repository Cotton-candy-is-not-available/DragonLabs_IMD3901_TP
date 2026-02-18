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
    //[SerializeField] private UnityEvent pinataForce;

    public float hitForce = 0.0f;
    public float maxHitForce = 100.0f;
    public PiñataController piñataController_access;


    private void Update()
    {

        if (Input.GetKey(selectKey)) //if LMB is being held down
        {
            indicatorTimer -= Time.deltaTime;
            hitChargeCircle.enabled = true;
            hitChargeCircle.fillAmount = indicatorTimer;

            //for looping its charge again
            if (indicatorTimer <= 0)
            {
                float normalizedCharge = 1f - (indicatorTimer / maxIndicatorTimer);
                normalizedCharge = Mathf.Clamp01(normalizedCharge);
                hitForce = normalizedCharge * maxHitForce;

                shouldUpdate = false;
                indicatorTimer = maxIndicatorTimer;
                hitChargeCircle.fillAmount = maxIndicatorTimer;
                hitChargeCircle.enabled = false;

                //call function that applies force to piñata
                piñataController_access.applyHitChargeForce(hitForce);
            }
        }
        else //if the LMB is NOT being held down anymore
        {
            if(shouldUpdate == true)
            {
                indicatorTimer += Time.deltaTime;
                hitChargeCircle.fillAmount = indicatorTimer;

                if(indicatorTimer >= maxIndicatorTimer)
                {
                    indicatorTimer = maxIndicatorTimer;
                    hitChargeCircle.fillAmount = maxIndicatorTimer;
                    hitChargeCircle.enabled = false;
                    shouldUpdate = false;
                }
            }
        }

        if (Input.GetKey(selectKey))
        {
            shouldUpdate = true;
        }
    }
}
