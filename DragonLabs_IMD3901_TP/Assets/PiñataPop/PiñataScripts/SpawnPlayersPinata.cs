using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPlayersPinata : NetworkBehaviour
{
    
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
    
}
