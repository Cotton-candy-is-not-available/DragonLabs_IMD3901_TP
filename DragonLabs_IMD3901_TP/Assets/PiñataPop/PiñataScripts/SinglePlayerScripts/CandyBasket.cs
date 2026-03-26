using UnityEngine;

public class CandyBasket : MonoBehaviour
{
    public GameObject basketP1;

    public ScoresManager scoresManager_access;
    public TimerController timerController_access;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("candy landed in basket");
            if(timerController_access.isExtraTimeDone == false) //only increase candy points if there's still extra time
            {
                scoresManager_access.increaseP1Candy();
            }
        }
    }

}
