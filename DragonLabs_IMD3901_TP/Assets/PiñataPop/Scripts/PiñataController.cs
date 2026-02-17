using UnityEngine;


public class PiñataController : MonoBehaviour
{

    int piñataHealth = 50;
    public bool isGameOver = false;
    public ScoresManager scoresManager_access;

    void Update()
    {
        Debug.Log("pinata health: " + piñataHealth);

        if (piñataHealth == 0)
        {
            Debug.Log("GAME OVER!");
            isGameOver = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.name == "BatP1")
        {
            Debug.Log("P1 hit the piñata");
            scoresManager_access.increaseP1Hits();
            if(piñataHealth > 0 && isGameOver == false)
            {
                piñataHealth -= 1;
            }
        }
        else if(collision.gameObject.name == "BatP2")
        {
            Debug.Log("P2 hit the piñata");
            scoresManager_access.increaseP2Hits();
            if (piñataHealth > 0 && isGameOver == false)
            {
                piñataHealth -= 1;
            }
        }
    }
}
