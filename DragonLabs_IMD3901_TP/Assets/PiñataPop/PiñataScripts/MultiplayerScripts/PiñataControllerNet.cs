using System.Collections;
using Unity.Netcode;
using UnityEngine;


public class PiñataControllerNet : NetworkBehaviour
{
    public NetworkVariable<int> piñataHealth;
    Rigidbody piñata_RB;

    public ScoresManagerNet scoresManagerNet_access;
    public ParticleSystem confettiPopParticles;
    public CandySpawn candySpawner_access;

    public WinBoardSpawn winBoardSpawn_access;

    public NetworkVariable<bool> isGameOver;
    bool shouldApplyForce = false;

    private void Start()
    {
        //fetch the pinata's rigid body
        piñata_RB = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        //set initial value
        piñataHealth.Value = 10;
        isGameOver.Value = false;

        //upate the values when they are changed
        piñataHealth.OnValueChanged += OnHealthPointsChanged;

        //set initial value of all the texts when the object spawns
        UpdateHealthPoints(piñataHealth.Value);
    }

    private void Update()
    {
        if (!IsServer) return;

        Debug.Log("pinata health: " + piñataHealth.Value);

        //only play confetti particle if the game is over and the pinata health is 0
        if (piñataHealth.Value <= 0 && isGameOver.Value == false) 
        {
            Debug.Log("GAME OVER!");
            isGameOver.Value = true;
            //display the winner board
            if (IsServer)
            {
                winBoardSpawn_access.SpawnWinBoardServerRpc();
            }

            //play confetti particles
            playConfettiServerRpc();
            candySpawner_access.SpawnCandyServerRpc();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "BatP1")
        {
            //Debug.Log("P1 hit the piñata");

            if (isGameOver.Value == false) //only increase points if game is not over
            {
                scoresManagerNet_access.addP1HitPointServerRpc();
            }

            shouldApplyForce = true;
            if (piñataHealth.Value > 0 && isGameOver.Value == false)
            {
                piñataHealth.Value -= 1;
            }
        }
        else if(collision.gameObject.name == "BatP2")
        {
            //Debug.Log("P2 hit the piñata");
            if (isGameOver.Value == false)
            {
                scoresManagerNet_access.addP2HitPointServerRpc();
            }

            shouldApplyForce = true;
            if (piñataHealth.Value > 0 && isGameOver.Value == false)
            {
                piñataHealth.Value -= 1;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void applyHitChargeForceServerRpc(float hitChargeForce)
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

    //PINATA HEALTH POINTS---------
    private void OnHealthPointsChanged(int oldValue, int newValue)
    {
        UpdateHealthPoints(newValue);
    }
    private void UpdateHealthPoints(int value)
    {
        piñataHealth.Value = value;
    }
    [ServerRpc(RequireOwnership = false)]
    public void subtractHealthPointServerRpc()
    {
        piñataHealth.Value -= 1;
        Debug.Log("subtracted a pinata health point");
    }

    [ServerRpc(RequireOwnership = false)]
    public void playConfettiServerRpc()
    {
        //play confetti particles for both the host and client
        confettiPopParticles.Play();
        playConfettiClientRpc();
    }
    [ClientRpc]
    public void playConfettiClientRpc()
    {
        confettiPopParticles.Play();
    }

}
