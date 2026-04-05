using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ballHitCups : NetworkBehaviour
{
    public NetworkVariable<bool> turnOffBall = new NetworkVariable<bool>(false);

    public gameManager manager;

    public AudioManager audioManager;
    public override void OnNetworkSpawn()
    {

        audioManager = FindFirstObjectByType<AudioManager>(); //find the audio manager in the scene

        manager = GameObject.Find("BeerPongGameManager").GetComponent<gameManager>(); //find the gameManager in the scene

        audioManager.PlaySFXServerRpc(AudioManager.SFXList.BallSpawn);//play SFX

    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "floor")//if ball touches the floor or table
        {
            audioManager.PlaySFXServerRpc(AudioManager.SFXList.BallFloor);//play SFX

            Debug.Log("Floor");
           
            manager.despawnBallServerRpc();


        }

        if (collision.gameObject.tag == "table")//if ball touches the floor or table
        {
            audioManager.PlaySFXServerRpc(AudioManager.SFXList.BallTable);//play SFX

            Debug.Log("table");
           
            manager.despawnBallServerRpc();


        }




    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "cup1")//if the ball hits player 1 Cup 
        {

            //manager.changeTurnRpc(2);//now player 2's turn
            Debug.Log("cup1");

            audioManager.PlaySFXServerRpc(AudioManager.SFXList.BallBeer);//play SFX

            manager.increaseP2PointsServerRpc();//increase player 2 points

            manager.despawnBallServerRpc();
            //gameObject.SetActive(turnOffBall.Value);


        }

        else if (trigger.gameObject.tag == "cup2")//player 2 cup 
        {
            //manager.changeTurnRpc(1);//now player 2's turn
            Debug.Log("cup2");
            audioManager.PlaySFXServerRpc(AudioManager.SFXList.BallBeer);//play SFX

            manager.increaseP1PointsServerRpc();//increase player 1 points

            manager.despawnBallServerRpc();


        }
    }


    private void Update()
    {
        if (gameObject.transform.position.y < 0)
        {
            manager.despawnBallServerRpc();
            Debug.Log("OUT OF BOUNDS");

           
        }
    }


  

}