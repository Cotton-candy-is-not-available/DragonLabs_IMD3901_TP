using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPlayersPinata : NetworkBehaviour
{
    // public NetworkObject player1;
    //public NetworkObject player2;

    public GameObject player1;
    public GameObject player2;

    public Transform p1StartPos;
    public Transform p2StartPos;


    private void Start()
    {
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");

        Debug.Log("host joined the scene");
        player1.transform.transform.position = p1StartPos.position;

        Debug.Log("client joined the scene");
        player2.transform.transform.position = p2StartPos.position;
    }

    /*
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }

            //find both player in the scene
            //player1 = GameObject.FindWithTag("Player1");
            //player2 = GameObject.FindWithTag("Player2");





            *//*
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
    *//*

}*/
}
