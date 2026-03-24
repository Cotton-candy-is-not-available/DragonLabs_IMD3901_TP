using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class gameManager : NetworkBehaviour
{
    GameObject P1;
    GameObject P2;

    public Canvas player1PointsCanvas;
    public Canvas player2PointsCanvas;

    [Header("------------ Points text -------------")]
    [Header("Player 1 view")]
    public TMP_Text P1YouPointsText;
    public TMP_Text P1OppPointsText;

    [Header("Player 2 view")]
    public TMP_Text P2YouPointsText;
    public TMP_Text P2OppPointsText;

    //[Header("------------ Points -------------")]
    public NetworkVariable<int> player1Points;
    public NetworkVariable<int> player2Points;
    //public int player1Points = 0;
    //public int player2Points = 0;

    [Header("------------ Ball start pos -------------")]
    public Vector3 P1BallStartPos;
    public Vector3 P2BallStartPos;

    [Header("------------ Prefabs -------------")]
    public NetworkObject[] ballPrefab;//prefab with network object attached


    //public beerPongScoreManger scoreManger;

    public NetworkVariable<int> turn;

    public GameObject player1;
    public GameObject player2;

    public Transform p1StartPos;
    public Transform p2StartPos;

    public NetworkObject newBall;

    public VolumeProfile playerVolumeProfile;

    public NetworkVariable<bool> isGameOver;
    public NetworkVariable<bool> goToLobby;


    public override void OnNetworkSpawn()
    {
        turn.Value = 1;
        isGameOver.Value = false;
        goToLobby.Value = false;

    }

    void Start()
    {
        //find both player in the scene
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");

        //Set players start positions
        player1.transform.transform.position = p1StartPos.position;
        player2.transform.transform.position = p2StartPos.position;

        //---------------- Post processign ------------------------//
        //add volume component to them so the blur/drunk effect can be called; this will be deleted when they leave the scenes
        //maybe check if VR player or PC
        //player1.AddComponent<Volume>();
        //player2.AddComponent<Volume>();

        ////set their volume profiles
        //player1.GetComponent<Volume>().profile = playerVolumeProfile;
        //player2.GetComponent<Volume>().profile = playerVolumeProfile;

       

        //---------------------------------------------------------//


        //Set ball start positions
        P1BallStartPos =  new Vector3(0f, 4.5f, -5f);
        P2BallStartPos = new Vector3(0f, 4.5f, 5);
        Debug.Log("Start turn: "+ turn.Value);

        //turn.Value = 1;
        spawnBallServerRpc(P1BallStartPos);//spawn the ball infornt of player 1
    }

    // Update is called once per frame
    void Update()
    {
 
        //Set their start positions
        player1.transform.transform.position = p1StartPos.position;
        player2.transform.transform.position = p2StartPos.position;
        Debug.Log("newBallIsPawned: " + newBall.IsSpawned);


        //instatiate ball depending on who's turn it is
        if (turn.Value == 1 && !newBall.IsSpawned)//if player 1 turn
        {
            spawnBallServerRpc(P1BallStartPos);//spawn the ball infornt of player 1
            turn.Value = 2;//now player 2's turn
            Debug.Log("Turn: "+ turn.Value);
        }
        else if (turn.Value == 2  && !newBall.IsSpawned)//if player 2 turn
        {
            spawnBallServerRpc(P2BallStartPos);//spawn the ball infront of player 2
            turn.Value = 1;//now player 1's turn
            Debug.Log("Turn: "+ turn.Value);

        }

        if (isGameOver.Value == true)
        {
            Debug.Log("Game is over");
        }
        //addPointOnDespawnServerRpc();

        //if (newBall != null)//if tha ball exists start checking if points are to be added
        //{
        //    ballHitCups ballHit = newBall.GetComponent<ballHitCups>();//get the ball collision check script


        //    //scoreManger.addP1PointServerRpc();



        //    if (ballHit.P1Point)
        //    {
        //        player1Points+=1;//increase points for player 1


        //        //P1
        //P1OppPointsText.text = ("Them: "+ player2Points.Value);//update Player 1's text 
        //P1YouPointsText.text = ("You: "+ player1Points.Value);//update Player 1's text 
        //Debug.Log("Player 1 points" +  player1Points.Value);
        //Debug.Log("Player 2 points" +  player2Points.Value);

        ////        //P2
        //P2OppPointsText.text = ("Them: "+ player1Points.Value);//update player 2's  text 
        //P2YouPointsText.text = ("You: "+ player2Points.Value);//update player 2's text 

        //        Debug.Log("player1Points: "+ player1Points);
        //        ballHit.P1Point = false;//reset to false
        //        despawnBallServerRpc();//destroy the ball

        //    }

        //    if (ballHit.P2Point)
        //    {
        //        player2Points+=1;//increase points for player 2
        //        //P1
        //        player1Points+=1;//increase points for player 1
        //        P1OppPointsText.text = ("Them: "+ player2Points);//update Player 1's text 
        //        P1YouPointsText.text = ("You: "+ player1Points);//update Player 1's text 

        //        //P2
        //        P2OppPointsText.text = ("Them: "+ player1Points);//update player 2's  text 
        //        P2YouPointsText.text = ("You: "+ player2Points);//update player 2's text 


        //        Debug.Log("player2Points: "+ player2Points);
        //        ballHit.P2Point = false;//reset to false
        //        despawnBallServerRpc();//destroy the ball

        //    }

        //    if (ballHit.nonCup)//destroy ball if it hits anything else for long enough
        //    {

        //        despawnBallServerRpc();//destroy the ball
        //        ballHit.nonCup = false;//turn it baxck off
        //    }


    }






    //Ball spawning and despawning
    [ServerRpc(RequireOwnership = false)]
    void spawnBallServerRpc(Vector3 startPos)//allows client to also spawn/ see spawned ball
    {
        
        foreach (NetworkObject ball in ballPrefab)
        {

            if (!IsServer) return;
            //NetworkObject newBall = Instantiate(ball, startPos, Quaternion.identity);
            newBall = Instantiate(ball, startPos, Quaternion.identity);
            newBall.GetComponent<NetworkObject>().Spawn();
        }
        
    }


    [ServerRpc(RequireOwnership = false)]
    public void despawnBallServerRpc()
    {

        foreach (NetworkObject ball in ballPrefab)
        {
            if (ball != null)
            {
                ball.Despawn();
            }
        }
    }



    





}