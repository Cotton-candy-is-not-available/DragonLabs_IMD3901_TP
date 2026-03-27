using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ballHitCups : NetworkBehaviour
{
    public static ballHitCups Instance;


    //public NetworkVariable<bool> P1Point;
    //public NetworkVariable<bool> P2Point;
    //public NetworkVariable<bool> nonCup;

    public gameManager manager;
    public override void OnNetworkSpawn()
    {

        //P1Point.Value = false;
        //P2Point.Value = false;

        //nonCup.Value = false;

        if (Instance != null)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();

        }
        else
        {
            Instance = this;

        }
        manager = GameObject.Find("BeerPongGameManager").GetComponent<gameManager>(); ;
    }

    private void OnCollisionEnter(Collision collision)
    {
      

        //if (collision.gameObject.tag == "floor" || collision.gameObject.tag == "table")//if ball touches the floor or table
        if (collision.gameObject.tag == "floor" )//if ball touches the floor or table
        {
            //nonCup.Value = true;//to be used when it is thrown and has not hit any beer/cups so it needs to reset
            gameObject.GetComponent<NetworkObject>().Despawn();

        }

  


    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.tag == "cup1")//if the ball hits player 1 Cup 
        {
            manager.changeTurnRpc(2);//now player 2's turn

            despawnBallServerRpc();

        }

        else if (trigger.gameObject.tag == "cup2")//player 2 cup 
        {
            manager.changeTurnRpc(1);//now player 2's turn

            //gameObject.GetComponent<NetworkObject>().Despawn();//destroy the ball
            despawnBallServerRpc();

        }
    }


    private void Update()
    {
        if(gameObject.transform.position.y < 0)
        {
            despawnBallServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void despawnBallServerRpc()
    {
        despawnBallClientRpc();
    }

    [ClientRpc]
    public void despawnBallClientRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();//destroy ball when it goes anywhere below floor level

    }

}
