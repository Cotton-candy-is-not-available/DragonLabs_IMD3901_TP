using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class SpawnPlayersPinata : NetworkBehaviour
{
    public NetworkObject player1;
    public NetworkObject player2;

    //public Transform p1StartPos;
    //public Transform p2StartPos;

    public NetworkObject p1StartPos;
    public NetworkObject p2StartPos;

    private void Update()
    {

        if (!IsServer)
        {
            return;
        }

        if (NetworkManager.IsServer)
        {
            player1 = GetComponent<NetworkObject>();
            Debug.Log("host joined the scene");
        }
        else if (NetworkManager.IsClient && !IsServer)
        {

            player2 = GetComponent<NetworkObject>();
            Debug.Log("client joined the scene");

        }



        /*  //check if the players have both spawned in the scene
          if (player1 != null)
          {
              spawnP1();
          }

          if (player2 != null)
          {
              spawnP2();
          }
  */

    }


    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        //find both player in the scene
        //player1 = GameObject.FindWithTag("Player1");
        //player2 = GameObject.FindWithTag("Player2");

        



        /*
        //spawning the players at their given spawn points
        if (NetworkManager.Singleton.LocalClientId == 0) //host
        {
            Debug.Log("placed host at spawn point");
            //player1.transform.transform.position = p1StartPos.position;
            player1.transform.transform.position = p1StartPos.transform.position;

        }
        else if (NetworkManager.Singleton.LocalClientId == 1) //client
        {
            Debug.Log("placed client at spawn point");
            //player2.transform.transform.position = p2StartPos.position;
            player2.transform.transform.position = p2StartPos.transform.position;
        }*/

        /*
            private void spawnP1()
            {
                Debug.Log("placed host at spawn point");
                //player1.transform.transform.position = p1StartPos.position;
                player1.transform.transform.position = p1StartPos.transform.position;

            }

            private void spawnP2()
            {
                Debug.Log("placed client at spawn point");
                //player2.transform.transform.position = p2StartPos.position;
                player2.transform.transform.position = p2StartPos.transform.position;
            }
        */

    }
}
