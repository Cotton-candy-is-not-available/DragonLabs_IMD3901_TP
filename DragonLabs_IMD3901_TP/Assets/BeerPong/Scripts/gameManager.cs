using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class gameManager : MonoBehaviour
{

    public TMP_Text player1PointsText;
    public TMP_Text player2PointsText;

    public int player1Points = 0;
    public int player2Points = 0;


    public Vector3 BallStartPos;
    public Vector3 P1BallStartPos;
    public Vector3 P2BallStartPos;

    public GameObject ballPrefab;

    public GameObject newBall;

    int turn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            ballHitCups ballHit = newBall.GetComponent<ballHitCups>();//get the ball collision chekc script

            if (ballHit.P1Point == true)
            {
                player1Points+=1;//increase points for player 1
                player1PointsText.text = ("You: "+ player1Points);//update text
                Debug.Log("player1Points: "+ player1Points);
                ballHit.P1Point = false;
                Destroy(newBall);//destroy the ball

            }

            if (ballHit.P2Point == true)
            {
                player2Points+=1;//increase points for player 2
                player2PointsText.text = ("Them: "+ player2Points);//update text
                Debug.Log("player2Points: "+ player2Points);
                ballHit.P2Point = false;
                Destroy(newBall);//destroy the ball
            }
        }
    }
}
