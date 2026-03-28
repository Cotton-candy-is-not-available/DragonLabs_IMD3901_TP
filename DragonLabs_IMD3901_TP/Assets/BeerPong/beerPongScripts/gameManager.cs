using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class gameManager : NetworkBehaviour
{
    public NetworkObject newBall;

    public Canvas player1PointsCanvas;
    public Canvas player2PointsCanvas;


    //[Header("------------ Points -------------")]
    public NetworkVariable<int> player1Points = new NetworkVariable<int>(0);
    public NetworkVariable<int> player2Points = new NetworkVariable<int>(0);
    //public int player1Points = 0;
    //public int player2Points = 0;

    [Header("------------ Ball start pos -------------")]
    public Vector3 P1BallStartPos;
    public Vector3 P2BallStartPos;

    [Header("------------ Prefabs -------------")]
    public NetworkObject[] ballPrefab;//prefab with network object attached


    //public beerPongScoreManger scoreManger;

    public NetworkVariable<int> turn = new NetworkVariable<int>(2);

    public GameObject player1;
    public GameObject player2;

    public Transform p1StartPos;
    public Transform p2StartPos;

    //public NetworkObject newBall;

    public VolumeProfile playerVolumeProfile;

    public NetworkVariable<bool> isGameOver = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> activateWinnerPanel = new NetworkVariable<bool>(false);
    //public NetworkVariable<bool> goToLobby;

    [Header("------------ Winner panels -------------")]
    public GameObject P1WinnerPanel;
    public GameObject P2WinnerPanel;


    public ChooseGame sceneManager;

    public override void OnNetworkSpawn()
    {
        turn.OnValueChanged += OnChangedTurn;
        isGameOver.OnValueChanged += OnGameOver;

        activateWinnerPanel.OnValueChanged += OnWinner;

        player1Points.OnValueChanged += OnAddPointP1;
        player2Points.OnValueChanged += OnAddPointP2;
       



    }

    void Start()
    {
        //find both player in the scene
        player1 = GameObject.FindWithTag("Player1");
        //player2 = GameObject.FindWithTag("Player2");

        //Set players start positions
        player1.transform.transform.position = p1StartPos.position;
        //player2.transform.transform.position = p2StartPos.position;

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
        //player2.transform.transform.position = p2StartPos.position;
        Debug.Log("newBallIsPawned: " + newBall);


        //instatiate ball depending on who's turn it is
        if (turn.Value == 1 && !newBall.IsSpawned)//if player 1 turn
        {
            spawnBallServerRpc(P1BallStartPos);//spawn the ball infornt of player 1
            //turn.Value = 2;//now player 2's turn
            changeTurnRpc(2);//now player 2's turn
            Debug.Log("NOW player 1: "+ turn.Value);
        }
        else if (turn.Value == 2 && !newBall.IsSpawned)//if player 2 turn
        {
            spawnBallServerRpc(P2BallStartPos);//spawn the ball infront of player 2
            //turn.Value = 1;//now player 1's turn
            changeTurnRpc(1);//now player 1's turn

            Debug.Log("NOW player 2: "+ turn.Value);

        }

        Debug.Log("player1Points.Value: " + player1Points.Value);
        Debug.Log("player2Points.Value: " + player2Points.Value);


        if (player1Points.Value == 3)
        {
            Debug.Log("Game is over");
            //isGameOver.Value == true;
            displayWinnerRpc();//set to true
            P1WinnerPanel.SetActive(activateWinnerPanel.Value);
            StartCoroutine(WaitToGoBack());//wait some seconds ebfor switching players back to lobby
            sceneManager.switchScenesNetServerRpc("Lobby");//bring players back to the lobby

        }
        else if (player2Points.Value == 3)
        {
            Debug.Log("Game is over");
            //isGameOver.Value == true;
            displayWinnerRpc();//set to true
            P2WinnerPanel.SetActive(activateWinnerPanel.Value);
            StartCoroutine(WaitToGoBack());//wait some seconds ebfor switching players back to lobby

            sceneManager.switchScenesNetServerRpc("Lobby");//bring players back to the lobby

        }
       


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
            //Debug.Log("newBall: ", newBall);
            //Debug.Log("newBall is spawned: "+ newBall.IsSpawned);
        }
        
    }


    //change turn value
    [Rpc(SendTo.Owner)]
    public void changeTurnRpc(int turnNum)
    {
        turn.Value = turnNum;
        Debug.Log("chnage rpc turn: " + turn.Value);

    }


    //change activateWinnerPanel value
    [Rpc(SendTo.Owner)]
    void displayWinnerRpc()
    {
        activateWinnerPanel.Value = true;
    }
    IEnumerator WaitToGoBack()
    {

        yield return new WaitForSeconds(5); //waits 5 seconds

    }

    //on changed for netvariables
    private void OnChangedTurn(int previous, int current)
    {
        Debug.Log($"Detected NetworkVariable Change Turn : Previous: {previous} | Current: {current}");
    }

    private void OnGameOver(bool previous, bool current)
    {
        Debug.Log($"Detected NetworkVariable Change Game Over: Previous: {previous} | Current: {current}");
    }

    private void OnWinner(bool previous, bool current)
    {
        Debug.Log($"Detected NetworkVariable Change Winner: Previous: {previous} | Current: {current}");
    }


    private void OnAddPointP1(int previous, int current)
    {
        Debug.Log($"Detected NetworkVariable Change add point P1: Previous: {previous} | Current: {current}");
    }

    private void OnAddPointP2(int previous, int current)
    {
        Debug.Log($"Detected NetworkVariable Change add point P2: Previous: {previous} | Current: {current}");
    }
}