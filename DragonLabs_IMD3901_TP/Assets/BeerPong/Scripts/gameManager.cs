using GogoGaga.OptimizedRopesAndCables;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class gameManager : MonoBehaviour
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

    [Header("------------ Points -------------")]
    public int player1Points = 0;
    public int player2Points = 0;

    [Header("------------ Ball start pos -------------")]
    public Vector3 P1BallStartPos;
    public Vector3 P2BallStartPos;

    [Header("------------ Prefabs -------------")]
    public GameObject ballPrefab;

    GameObject newBall;

    int turn;

    void Start()
    {
     turn = 1;
     P1BallStartPos =  new Vector3(0.0109999999f, 1.60599995f, -6.93300009f);

     P2BallStartPos = new Vector3(0.0109999999f, 1.60599995f, 3.11f);
     Debug.Log("Start turn: "+ turn);
     newBall = Instantiate(ballPrefab, P1BallStartPos, Quaternion.identity);//instatiate ball infront of player1

    }

    // Update is called once per frame
    void Update()
    {
       //instatiate ball depending on who's turn it is
        if (newBall == null && turn == 1)//if player 1 turn
        {
           newBall = Instantiate(ballPrefab, P2BallStartPos, Quaternion.identity);//instatiate ball infront of player2
           turn = 2;//now player 2's turn
           Debug.Log("Turn: "+ turn);
        }
        else if (newBall == null && turn == 2)//if player 2 turn
        {
            newBall = Instantiate(ballPrefab, P1BallStartPos, Quaternion.identity);//instatiate ball infront of player2
            turn = 1;//now player 1's turn
            Debug.Log("Turn: "+ turn);

        }

        if (newBall != null)//if tha ball exists start checking if points are to be added
        {
            ballHitCups ballHit = newBall.GetComponent<ballHitCups>();//get the ball collision check script

            if (ballHit.P1Point)
            {
                player1Points+=1;//increase points for player 1
                P1YouPointsText.text = ("You: "+ player1Points);//update text player 1's side
                P2OppPointsText.text = ("Them: "+ player1Points);//update text on layer 2's Side

                Debug.Log("player1Points: "+ player1Points);
                ballHit.P1Point = false;
                Destroy(newBall);//destroy the ball

            }

            if (ballHit.P2Point)
            {
                player2Points+=1;//increase points for player 2
                P1OppPointsText.text = ("Them: "+ player2Points);//update text on player 1's side
                P2YouPointsText.text = ("You: "+ player2Points);//update text on layer 2's Side

                Debug.Log("player2Points: "+ player2Points);
                ballHit.P2Point = false;
                Destroy(newBall);//destroy the ball
            }

            if (ballHit.nonCup)//destroy ball if it hits anything else for long enough
            {
                //might need to check when thrown?
                Destroy(newBall);//destroy the ball
            }
        }
    }
}
