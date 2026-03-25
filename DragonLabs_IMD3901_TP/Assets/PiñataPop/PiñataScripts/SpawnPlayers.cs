using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class SpawnPlayers : NetworkBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public Transform p1StartPos;
    public Transform p2StartPos;

    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            return;
        }

        //find both player in the scene
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");

        /*//Set players start positions
        player1.transform.transform.position = p1StartPos.position;
        player2.transform.transform.position = p2StartPos.position;*/

        //spawning the players at their given spawn points
        if (NetworkManager.Singleton.LocalClientId == 0) //host
        {
            Debug.Log("placed host at spawn point");
            player1.transform.transform.position = p1StartPos.position;
        }
        else if (NetworkManager.Singleton.LocalClientId == 1) //client
        {
            Debug.Log("placed client at spawn point");
            player2.transform.transform.position = p2StartPos.position;
        }

    }










}
