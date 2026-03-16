using UnityEngine;

public class CandyBasket : MonoBehaviour
{
    public GameObject basketP1;

    public ScoresManager scoresManager_access;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("candy landed in basket");
            scoresManager_access.increaseP1Candy();
        }
    }

}
