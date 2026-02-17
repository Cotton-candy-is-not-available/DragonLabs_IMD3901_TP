using UnityEngine;


public class PiñataController : MonoBehaviour
{

    int piñataHealth = 50;
    public bool isGameOver = false;
    public ScoresManager scoresManager_access;
    public ParticleSystem confettiPopParticles;


    void Update()
    {
        Debug.Log("pinata health: " + piñataHealth);

        //only play confetti particle if the game is over and the pinata health is 0
        if (piñataHealth <= 0 && isGameOver == false) 
        {
            Debug.Log("GAME OVER!");
            isGameOver = true;
            confettiPopParticles.Play();
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
