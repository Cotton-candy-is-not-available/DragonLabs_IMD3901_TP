using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ballHitCups : NetworkBehaviour
{
    public static ballHitCups Instance;


    public NetworkVariable<bool> turnOffBall = new NetworkVariable<bool>(false);

    public gameManager manager;
    public override void OnNetworkSpawn()
    {

        //P1Point.Value = false;
        //P2Point.Value = false;

        ////nonCup.Value = false;

        //if (Instance != null)
        //{
        //    gameObject.GetComponent<NetworkObject>().Despawn();

        //}
        //else
        //{
        //    Instance = this;

        //}
        manager = GameObject.Find("BeerPongGameManager").GetComponent<gameManager>(); ;
    }

    private void OnCollisionEnter(Collision collision)
    {


        //if (collision.gameObject.tag == "floor" || collision.gameObject.tag == "table")//if ball touches the floor or table
        if (collision.gameObject.tag == "floor")//if ball touches the floor or table
        {
            Debug.Log("Floor");
            //nonCup.Value = true;//to be used when it is thrown and has not hit any beer/cups so it needs to reset
            //despawnBallServerRpc();
            //manager.newBall = null;
            //despawnBallRpc();
            //gameObject.SetActive(turnOffBall.Value);
            manager.despawnBallServerRpc();


        }

        if (collision.gameObject.tag == "table")//if ball touches the floor or table
        {
            Debug.Log("table");
            //nonCup.Value = true;//to be used when it is thrown and has not hit any beer/cups so it needs to reset
            //despawnBallServerRpc();
            //manager.newBall = null;
            //despawnBallRpc();
            //gameObject.SetActive(turnOffBall.Value);
            manager.despawnBallServerRpc();


        }




    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.tag == "cup1")//if the ball hits player 1 Cup 
        {
            //manager.changeTurnRpc(2);//now player 2's turn
            Debug.Log("cup1");

            //despawnBallServerRpc();
            //manager.newBall = null;
            //despawnBallRpc();
            manager.despawnBallServerRpc();
            //gameObject.SetActive(turnOffBall.Value);


        }

        else if (trigger.gameObject.tag == "cup2")//player 2 cup 
        {
            //manager.changeTurnRpc(1);//now player 2's turn
            Debug.Log("cup2");

            //gameObject.GetComponent<NetworkObject>().Despawn();//destroy the ball
            //despawnBallServerRpc();
            //despawnBallRpc();
            //manager.newBall = null;
            //gameObject.SetActive(turnOffBall.Value);

            manager.despawnBallServerRpc();


        }
    }


    private void Update()
    {
        if (gameObject.transform.position.y < 0)
        {
            manager.despawnBallServerRpc();

            //manager.newBall = null;
            //gameObject.SetActive(turnOffBall.Value);
            //despawnBallServerRpc();
            //despawnBallRpc();
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


    //[ServerRpc(RequireOwnership = false)]
    //public void despawnBallServerRpc()
    //{
    //    //despawnBallClientRpc();
    //}

    //[ClientRpc]
    //public void despawnBallClientRpc()
    //{
    //    if (IsServer)
    //    {
    //        Debug.Log("is server: despawn");
    //        StartCoroutine(WaitToDestroy());//destoy the ball

    //        gameObject.GetComponent<NetworkObject>().Despawn();//destroy ball when it goes anywhere below floor level
    //    }
    //    else if (!IsServer)
    //    {
    //        return;
    //    }
    //}


    IEnumerator WaitToDestroy()
    {
       
        yield return new WaitForSeconds(5); //waits 3 seconds

    }
}