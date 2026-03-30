using System.Collections;
using Unity.Netcode;
using UnityEngine;


public class PinataController : MonoBehaviour
{

    int pinataHealth = 10;
    Rigidbody pinata_RB;

    public ScoresManager scoresManager_access;
    public ParticleSystem confettiPopParticles;
    public AudioManagerSinglePlayer audioManSinglePlayer;

    public GameObject candy;

    public bool isGameOver = false;
    bool shouldApplyForce = false;

    private void Start()
    {
        //fetch the pinata's rigid body
        pinata_RB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log("pinata health: " + pinataHealth);

        //only play confetti particle if the game is over and the pinata health is 0
        if (pinataHealth <= 0 && isGameOver == false) 
        {
            Debug.Log("GAME OVER!");
            isGameOver = true;
            confettiPopParticles.Play();
            audioManSinglePlayer.PlaySFX(audioManSinglePlayer.pinataPopSound);
            audioManSinglePlayer.PlaySFX(audioManSinglePlayer.partyBlower);
            candy.SetActive(true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "BatP1")
        {
            audioManSinglePlayer.PlaySFX(audioManSinglePlayer.batHitSound);
            //Debug.Log("P1 hit the pinata");
            if(isGameOver == false) //only add point if the game is not over
            {
                scoresManager_access.increaseP1Hits();
            }
            shouldApplyForce = true;
            if (pinataHealth > 0 && isGameOver == false)
            {
                pinataHealth -= 1;
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
            pinata_RB.AddForce(transform.up * hitChargeForce, ForceMode.Impulse);
            shouldApplyForce = false; //reset
        }
    }
}
