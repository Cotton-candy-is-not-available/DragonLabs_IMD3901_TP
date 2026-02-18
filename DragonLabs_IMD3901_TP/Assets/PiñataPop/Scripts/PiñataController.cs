using UnityEngine;


public class PiñataController : MonoBehaviour
{

    int piñataHealth = 50;
    Rigidbody piñata_RB;

    public ScoresManager scoresManager_access;
    public ParticleSystem confettiPopParticles;
    
    public bool isGameOver = false;
    bool shouldApplyForce = false;

    private void Start()
    {
        //fetch the pinata's rigid body
        piñata_RB = GetComponent<Rigidbody>();
    }

    private void Update()
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
            //Debug.Log("P1 hit the piñata");
            scoresManager_access.increaseP1Hits();
            shouldApplyForce = true;
            if (piñataHealth > 0 && isGameOver == false)
            {
                piñataHealth -= 1;
            }
        }
        else if(collision.gameObject.name == "BatP2")
        {
            //Debug.Log("P2 hit the piñata");
            scoresManager_access.increaseP2Hits();
            shouldApplyForce = true;
            if (piñataHealth > 0 && isGameOver == false)
            {
                piñataHealth -= 1;
            }
        }
    }

    public void applyHitChargeForce(float hitChargeForce)
    {
        /*ensure that the bat is colliding with the pinata at the same time as
        when the hit charge was released*/
        if (shouldApplyForce == true) 
        {
            Debug.Log("APPLIED FORCE OF: " + hitChargeForce + " TO PINATA");
            hitChargeForce = Mathf.Clamp(hitChargeForce, 0f, 30f); //min 0 and max 30
            //apply force in the Y direction to mimic someone pulling the pinata up
            piñata_RB.AddForce(transform.up * hitChargeForce, ForceMode.Impulse);
            shouldApplyForce = false; //reset
        }
    }


}
