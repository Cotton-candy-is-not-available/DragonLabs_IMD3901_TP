using GogoGaga.OptimizedRopesAndCables;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEditor.ShaderGraph.Internal;
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
    public GameObject ballPrefab;

    [SerializeField] NetworkObject newBall;

    float remainingTime = 3.0f;


    //public beerPongScoreManger scoreManger;

    int turn;

    void Start()
    {
        //turn = 0;//prevents game from starting
        turn = 1;
        P1BallStartPos =  new Vector3(0f, 3f, -5f);

        P2BallStartPos = new Vector3(0f, 3f, 5);
        Debug.Log("Start turn: "+ turn);
    }

    // Update is called once per frame
    void Update()
    {
        //if (turn == 0 &&  newBall == null)
        //{
        //    spawnBallServerRPC(P1BallStartPos);//spawn the ball infornt of player 1
        //    //turn = 1;

        //}

            //instatiate ball depending on who's turn it is
            if (newBall == null && turn == 1)//if player 1 turn
            {
                spawnBallServerRPC(P1BallStartPos);//spawn the ball infornt of player 1
                turn = 2;//now player 2's turn
                Debug.Log("Turn: "+ turn);
            }
            else if (newBall == null && turn == 2)//if player 2 turn
            {
                spawnBallServerRPC(P2BallStartPos);//spawn the ball infront of player 2

                turn = 1;//now player 1's turn
                Debug.Log("Turn: "+ turn);

            }

        if (newBall != null)//if tha ball exists start checking if points are to be added
        {
            ballHitCups ballHit = newBall.GetComponent<ballHitCups>();//get the ball collision check script


            //scoreManger.addP1PointServerRpc();



            if (ballHit.P1Point)
            {
                player1Points+=1;//increase points for player 1


                //P1
                P1OppPointsText.text = ("Them: "+ player2Points);//update Player 1's text 
                P1YouPointsText.text = ("You: "+ player1Points);//update Player 1's text 

                //P2
                P2OppPointsText.text = ("Them: "+ player1Points);//update player 2's  text 
                P2YouPointsText.text = ("You: "+ player2Points);//update player 2's text 

                Debug.Log("player1Points: "+ player1Points);
                ballHit.P1Point = false;//reset to false
                despawnBallServerRPC();//destroy the ball

            }

            if (ballHit.P2Point)
            {
                player2Points+=1;//increase points for player 2
                //P1
                player1Points+=1;//increase points for player 1
                P1OppPointsText.text = ("Them: "+ player2Points);//update Player 1's text 
                P1YouPointsText.text = ("You: "+ player1Points);//update Player 1's text 

                //P2
                P2OppPointsText.text = ("Them: "+ player1Points);//update player 2's  text 
                P2YouPointsText.text = ("You: "+ player2Points);//update player 2's text 


                Debug.Log("player2Points: "+ player2Points);
                ballHit.P2Point = false;//reset to false
                despawnBallServerRPC();//destroy the ball

            }

            if (ballHit.nonCup)//destroy ball if it hits anything else for long enough
            {

                despawnBallServerRPC();//destroy the ball
                ballHit.nonCup = false;//turn it baxck off
            }

            //if (ballHit.table)
            //{
            //    //remainingTime = 3.0f;// start with 3 seconds
            //    CheckTimeOnTable();//start count down timer
            //    if (remainingTime <= 0)
            //    {
            //        despawnBallServerRPC();//destroy the ball
            //        //ballHit.table = false;//turn it back off
            //        remainingTime = 3.0f;//reset remaining time
            //        Debug.Log("remainingTime in if statement: " + remainingTime);

            //    }
            //    Debug.Log("after check function: " + remainingTime);



            //}
            //else if (!ballHit.table)
            //{
            //    remainingTime = 3.0f;//reset remaining time
            //    Debug.Log("table is false");
            //}
        
        }
        
    }






    //Ball spawning and despawning

    [ServerRpc(RequireOwnership = false)]
    void spawnBallServerRPC(Vector3 startPos)
    {
        newBall = Instantiate(ballPrefab,startPos, Quaternion.identity).GetComponent<NetworkObject>();//instatiate the object
        newBall.Spawn();//spawn it over the network
        Debug.Log("newBall: " + newBall);
    }

    [ServerRpc(RequireOwnership = false)]
    void despawnBallServerRPC()
    {
        //WaitAndDespawn();
        newBall.Despawn();//destroy the ball
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
