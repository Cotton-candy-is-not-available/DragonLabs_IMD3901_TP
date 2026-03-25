using Unity.Netcode;
using UnityEngine;

public class CandyBasketNet : NetworkBehaviour
{
    public GameObject basketP1;
    public GameObject basketP2;

    public ScoresManagerNet scoresManagerNet_access;
    public TimerControllerNet timerControllerNet_access;


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("candy landed in basket");
                                                 //only increase candy points if there's still extra time
            if (gameObject.name == "BasketP1" && timerControllerNet_access.isExtraTimeDone.Value == false)
            {
                scoresManagerNet_access.addP1CandyPointServerRpc();
            }
            else if (gameObject.name == "BasketP2" && timerControllerNet_access.isExtraTimeDone.Value == false)
            {
                scoresManagerNet_access.addP2CandyPointServerRpc();
            }
        }
    }

}
