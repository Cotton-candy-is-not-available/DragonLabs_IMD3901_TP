using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
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
    //public NetworkVariable<int> player1Points = new NetworkVariable<int>();
    //public NetworkVariable<int> player2Points = new NetworkVariable<int>();
    public int player1Points = 0;
    public int player2Points = 0;

    [Header("------------ Ball start pos -------------")]
    public Vector3 P1BallStartPos;
    public Vector3 P2BallStartPos;

    [Header("------------ Prefabs -------------")]
    //public GameObject ballPrefab;

    public NetworkObject[] ballPrefab;

    float remainingTime = 3.0f;


    //public beerPongScoreManger scoreManger;

    //int turn;
    public NetworkVariable<int> turn;

    public GameObject player1;
    public GameObject player2;

    public Transform p1StartPos;
    public Transform p2StartPos;

    public NetworkObject newBall;

    public override void OnNetworkSpawn()
    {
        turn.Value = 1;
        spawnBallServerRpc(P1BallStartPos);//spawn the ball infornt of player 1

    }

    void Start()
    {
        //////find both player in the scene
        //player1 = GameObject.FindWithTag("Player1");
        //player2 = GameObject.FindWithTag("Player2");

        ////Set their start positions
        //player1.transform.transform.position = p1StartPos.position;
        //player2.transform.transform.position = p2StartPos.position;


        P1BallStartPos =  new Vector3(0f, 4f, -5.756f);

        P2BallStartPos = new Vector3(0f, 4f, 5.756f);
        Debug.Log("Start turn: "+ turn.Value);

        //turn.Value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (turn == 0 &&  newBall == null)
        //{
        //    spawnBallServerRPC(P1BallStartPos);//spawn the ball infornt of player 1
        //    //turn = 1;

        //}
        //Set their start positions
        //player1.transform.transform.position = p1StartPos.position;
        //player2.transform.transform.position = p2StartPos.position;
        //Debug.Log("newBallIsPawned: " + newBall.IsSpawned);
    

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

       

        //addPointOnDespawnServerRpc();

        //if (newBall != null)//if tha ball exists start checking if points are to be added
        //{
        //    ballHitCups ballHit = newBall.GetComponent<ballHitCups>();//get the ball collision check script


        //    //scoreManger.addP1PointServerRpc();



        //    if (ballHit.P1Point)
        //    {
        //        player1Points+=1;//increase points for player 1


        //        //P1
        //        P1OppPointsText.text = ("Them: "+ player2Points);//update Player 1's text 
        //        P1YouPointsText.text = ("You: "+ player1Points);//update Player 1's text 

        //        //P2
        //        P2OppPointsText.text = ("Them: "+ player1Points);//update player 2's  text 
        //        P2YouPointsText.text = ("You: "+ player2Points);//update player 2's text 

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

        //    //if (ballHit.table)
        //    //{
        //    //    //remainingTime = 3.0f;// start with 3 seconds
        //    //    CheckTimeOnTable();//start count down timer
        //    //    if (remainingTime <= 0)
        //    //    {
        //    //        despawnBallServerRPC();//destroy the ball
        //    //        //ballHit.table = false;//turn it back off
        //    //        remainingTime = 3.0f;//reset remaining time
        //    //        Debug.Log("remainingTime in if statement: " + remainingTime);

        //    //    }
        //    //    Debug.Log("after check function: " + remainingTime);



        //    //}
        //    //else if (!ballHit.table)
        //    //{
        //    //    remainingTime = 3.0f;//reset remaining time
        //    //    Debug.Log("table is false");
        //    //}

        //}

    }






    //Ball spawning and despawning

    [ServerRpc(RequireOwnership = false)]
    void spawnBallServerRpc(Vector3 startPos)
    {
        //for (int i = 0; i < 1; i++)
        //{
            foreach (NetworkObject ball in ballPrefab)
            {

                if (!IsServer) return;
                //NetworkObject newBall = Instantiate(ball, startPos, Quaternion.identity);
                newBall = Instantiate(ball, startPos, Quaternion.identity);
                newBall.GetComponent<NetworkObject>().Spawn();
            }
        //}
    }


    //void spawnBallServerRPC(Vector3 startPos)
    //{
    //    newBall = Instantiate(ballPrefab,startPos, Quaternion.identity).GetComponent<NetworkObject>();//instatiate the object
    //    newBall.Spawn();//spawn it over the network
    //    Debug.Log("newBall: " + newBall);
    //}

    [ServerRpc(RequireOwnership = false)]
    public void despawnBallServerRpc()
    {

        foreach (NetworkObject ball in ballPrefab)
        {
            //WaitAndDespawn();
            if (ball != null)
            {
                ball.Despawn();
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void addPointOnDespawnServerRpc()//add points to players
    {
        foreach (NetworkObject ball in ballPrefab)
        {
            if (ball.GetComponent<ballHitCups>().P1Point.Value)
            {
                Debug.Log("Player 1 add piont");
            }
            else
            {
                Debug.Log("No points");
            }
            //get ballHit component
            //despawn the ball


            //if (ball == null)//if the the ball landed in the cup and was despawned
            //{
                //if(ball)
            //}
        }
    }

    IEnumerator WaitAndDespawn()
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(5);
        //print("WaitAndPrint " + Time.time);
    }

    void CheckTimeOnTable()
    {
        remainingTime-= Time.deltaTime;//count down

        int minutes = Mathf.FloorToInt(remainingTime/60);
        int seconds = Mathf.FloorToInt(remainingTime%60);

        Debug.Log(" time cpunter " + string.Format("{0:00}:{1:00}", minutes, seconds));
        Debug.Log("remaining time" +remainingTime);


    }







}
