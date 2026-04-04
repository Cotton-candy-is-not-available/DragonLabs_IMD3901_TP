using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ballHitCups : NetworkBehaviour
{
    public NetworkVariable<bool> turnOffBall = new NetworkVariable<bool>(false);

    public gameManager manager;

    public AudioManager audioManager;
    public override void OnNetworkSpawn()
    {

        audioManager = FindFirstObjectByType<AudioManager>(); //find the audio manager in the scene

        manager = GameObject.Find("BeerPongGameManager").GetComponent<gameManager>(); //find the gameManager in the scene

        audioManager.PlaySFX(audioManager.ballSpawnSFX);//play SFX

    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "floor")//if ball touches the floor or table
        {
            audioManager.PlaySFX(audioManager.ballFloorSFX);//play SFX

            Debug.Log("Floor");
           
            manager.despawnBallServerRpc();


        }

        if (collision.gameObject.tag == "table")//if ball touches the floor or table
        {
            audioManager.PlaySFX(audioManager.ballTableSFX);//play SFX

            Debug.Log("table");
           
            manager.despawnBallServerRpc();


        }




    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "cup1")//if the ball hits player 1 Cup 
        {

            //manager.changeTurnRpc(2);//now player 2's turn
            Debug.Log("cup1");

            audioManager.PlaySFX(audioManager.ballBeerSFX);//play SFX

            manager.increaseP2PointsRpc();//increase player 2 points

            manager.despawnBallServerRpc();
            //gameObject.SetActive(turnOffBall.Value);


        }

        else if (trigger.gameObject.tag == "cup2")//player 2 cup 
        {
            //manager.changeTurnRpc(1);//now player 2's turn
            Debug.Log("cup2");
            audioManager.PlaySFX(audioManager.ballBeerSFX);//play SFX

            manager.increaseP1PointsRpc();//increase player 1 points

            manager.despawnBallServerRpc();


        }
    }


    private void Update()
    {
        if (gameObject.transform.position.y < 0)
        {
            manager.despawnBallServerRpc();
            Debug.Log("OUT OF BOUNDS");

           
        }
    }


    [ServerRpc(RequireOwnership =false)]
    public void despawnBallServerRpc()
    {
        if (!IsServer) return;
        Debug.Log("DESPAWN");
        StartCoroutine(WaitToDestroy());//destoy the ball

        NetworkObject netBall = gameObject.GetComponent<NetworkObject>();//destroy ball when it goes anywhere below floor level

        netBall.Despawn();
    }



    IEnumerator WaitToDestroy()
    {
       
        yield return new WaitForSeconds(5); //waits 3 seconds

    }
}