using System.Collections;
using Unity.Netcode;
using UnityEngine;


public class PiñataControllerNet : NetworkBehaviour
{

    int piñataHealth = 10;
    Rigidbody piñata_RB;

    public ScoresManagerNet scoresManagerNet_access;
    public ParticleSystem confettiPopParticles;
    public CandySpawn candySpawner_access;

    //public NetworkObject candy;
    //public NetworkObject[] candyList;

    public bool isGameOver = false;
    bool shouldApplyForce = false;

    private void Start()
    {
        //fetch the pinata's rigid body
        piñata_RB = GetComponent<Rigidbody>();
        //candy.GetComponent<NetworkObject>();

        //StartCoroutine(startDelay(20.0f));
    }

    private void Update()
    {
        if (!IsServer) return;

        Debug.Log("pinata health: " + piñataHealth);

        //only play confetti particle if the game is over and the pinata health is 0
        if (piñataHealth <= 0 && isGameOver == false) 
        {
            Debug.Log("GAME OVER!");
            isGameOver = true;
            //confettiPopParticles.Play();
            //candy.SetActive(true);
            //DropCandyServerRpc();

            //gameObject.GetComponent<CandySpawn>().SpawnCandyServerRpc();
            candySpawner_access.SpawnCandyServerRpc();

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "BatP1")
        {
            //Debug.Log("P1 hit the piñata");
            scoresManagerNet_access.addP1HitPointServerRpc();
            shouldApplyForce = true;
            if (piñataHealth > 0 && isGameOver == false)
            {
                piñataHealth -= 1;
            }
        }
        else if(collision.gameObject.name == "BatP2")
        {
            //Debug.Log("P2 hit the piñata");
            scoresManagerNet_access.addP2HitPointServerRpc();
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

   /* [ServerRpc(RequireOwnership = false)]
    public void DropCandyServerRpc()
    {
        //confettiPopParticles.Play();
        //candy.gameObject.SetActive(true);
       
        foreach (NetworkObject candy in candyList)
        {
            //candy.gameObject.SetActive(true);
            if (!candy.IsSpawned)
            {
                candy.Spawn(true);
                Debug.Log("candy spawned");
            }
        }
        
        Debug.Log("DropCandyServerRpc has been called");

        confettiPopParticles.Play();
        ShowCandyClientRpc();
    }

    [ClientRpc]
    public void ShowCandyClientRpc()
    {
        Debug.Log("ShowCandyClientRpc has been called");

        confettiPopParticles.Play();
        //candy.gameObject.SetActive(true);

    }*/


}
