using Unity.Netcode;
using UnityEngine;

public class CandyBasketNet : NetworkBehaviour
{
    public GameObject basketP1;
    public GameObject basketP2;

    public ScoresManagerNet scoresManagerNet_access;
    //PiñataController pinataController_acces;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("candy landed in basket");

            if(gameObject.name == "BasketP1")
            {
                scoresManagerNet_access.addP1CandyPointServerRpc();
            }
            else if (gameObject.name == "BasketP2")
            {
                scoresManagerNet_access.addP2CandyPointServerRpc();
            }
        }
    }

}
