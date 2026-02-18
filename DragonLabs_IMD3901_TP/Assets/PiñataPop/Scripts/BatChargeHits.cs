using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BatChargeHits : MonoBehaviour
{
    [SerializeField] private float indicatorTimer = 1.0f;
    [SerializeField] private float maxIndicatorTimer = 1.0f;
    [SerializeField] private Image hitChargeCircle;
    [SerializeField] private KeyCode selectKey = KeyCode.Mouse0; //left mouse click for PC
    [SerializeField] private bool shouldUpdate = false;
    [SerializeField] private UnityEvent pinataForce;
   
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
                shouldUpdate = false;
                indicatorTimer = maxIndicatorTimer;
                hitChargeCircle.fillAmount = maxIndicatorTimer;
                hitChargeCircle.enabled = false;
                pinataForce.Invoke();
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
