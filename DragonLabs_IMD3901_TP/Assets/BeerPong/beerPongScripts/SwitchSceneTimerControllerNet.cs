using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SwitchSceneTimerControllerNet : NetworkBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI timerDisplay;

    public NetworkVariable<float> elapsedTime;
    public ChooseGame sceneManager;
    public gameManager gameManagerAccess;//beer pong game manager

    public override void OnNetworkSpawn()
    {
        //set initial value
        elapsedTime.Value = 20.0f;
    }

    private void Update()
    {
        if (!IsServer) //only the server should be updating the elapsed time and doing the math
        {
            return;
        }
        UpdateTimerServerRpc(); //only update the client's display
    }


    [ServerRpc(RequireOwnership = false)]
    private void UpdateTimerServerRpc()
    {
        //Debug.Log("UpdateTimerServerRpc run");

        if (gameManagerAccess.isGameOver.Value == false) //if the game is not over the timer should be counting
        {
            elapsedTime.Value -= Time.deltaTime; //calculates all of the time passed since game started

            if (elapsedTime.Value <= 0)
            {
                elapsedTime.Value = 0;
                gameManagerAccess.isGameOver.Value = true;
            }

            int minutes = Mathf.FloorToInt(elapsedTime.Value / 60);
            int seconds = Mathf.FloorToInt(elapsedTime.Value % 60);

            //formats the minutes and seconds to display as 00:00
            timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            UpdateTimerClientRpc(); //update the client
        }
        else //if the game is over (either failure or victory), end the time
        {
            elapsedTime.Value = 0;
            timerDisplay.text = "00:00";
            UpdateTimerClientRpc(); //update the client
            sceneManager.switchScenesNetServerRpc("Lobby");
        }
    }

    [ClientRpc]
    public void UpdateTimerClientRpc()
    {
        //Debug.Log("updateTimerClientRpc run");

        int minutes = Mathf.FloorToInt(elapsedTime.Value / 60);
        int seconds = Mathf.FloorToInt(elapsedTime.Value % 60);

        //formats the minutes and seconds to display as 00:00
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
  
}