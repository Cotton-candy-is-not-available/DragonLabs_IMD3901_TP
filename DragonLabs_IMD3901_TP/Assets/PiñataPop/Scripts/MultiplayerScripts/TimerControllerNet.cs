using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TimerControllerNet : NetworkBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI timerDisplay;
    //public NetworkVariable<TMPro.TextMeshPro> timerDisplay;
    //float elapsedTime = 90.0f;

    public NetworkVariable<float> elapsedTime;
    public PiñataControllerNet piñataControllerNet_access;

    /*
    public override void OnNetworkSpawn()
    {
        //set initial value
        elapsedTime.Value = 90.0f;
    }
    */

    /*
    private void Update()
    {
        if(piñataControllerNet_access.isGameOver.Value == false)
        {
            UpdateTimeRegularServerRpc();
        }
        else if (piñataControllerNet_access.isGameOver.Value == true) //if the game is over (either failure or victory), reset the timer
        {
            UpdateTimeGameOverServerRpc();
        }
    }
    */

    void Update()
    {
        if (piñataControllerNet_access.isGameOver.Value == false) //if the game is not over the timer should be counting
        {
            elapsedTime.Value -= Time.deltaTime; //calculates all of the time passed since game started

            if (elapsedTime.Value <= 0)
            {
                elapsedTime.Value = 0;
                piñataControllerNet_access.isGameOver.Value = true;
            }

            int minutes = Mathf.FloorToInt(elapsedTime.Value / 60);
            int seconds = Mathf.FloorToInt(elapsedTime.Value % 60);

            //formats the minutes and seconds to display as 00:00
            timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else //if the game is over (either failure or victory), reset the timer
        {
            elapsedTime.Value = 0;
            timerDisplay.text = "00:00";
        }
    }

    //TIMER ELAPSED TIME---------
    /* private void OnTimeChanged(NetworkVariable<float> oldValue, NetworkVariable<float> newValue)
     {
         UpdateTime(newValue);
     }*/

    /*
    [ServerRpc(RequireOwnership = false)]
    private void UpdateTimeGameOverServerRpc()
    {
        elapsedTime.Value = 0;
        timerDisplay.text = "00:00";
        Debug.Log("UpdateTimeGameOverServerRpc run");
        UpdateClienttimer222ClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateTimeRegularServerRpc()
    {
        elapsedTime.Value -= Time.deltaTime; //calculates all of the time passed since game started

        if (elapsedTime.Value <= 0)
        {
            elapsedTime.Value = 0;
            piñataControllerNet_access.isGameOver.Value = true;
        }

        int minutes = Mathf.FloorToInt(elapsedTime.Value / 60);
        int seconds = Mathf.FloorToInt(elapsedTime.Value % 60);

        //formats the minutes and seconds to display as 00:00
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        Debug.Log("UpdateTimeRegularServerRpc run");
        updateTimerClientRpc();
    }

    [ClientRpc]
    public void updateTimerClientRpc()
    {
        Debug.Log("updateTimerClientRpc run");

        elapsedTime.Value -= Time.deltaTime; //calculates all of the time passed since game started

        if (elapsedTime.Value <= 0)
        {
            elapsedTime.Value = 0;
            piñataControllerNet_access.isGameOver.Value = true;
        }

        int minutes = Mathf.FloorToInt(elapsedTime.Value / 60);
        int seconds = Mathf.FloorToInt(elapsedTime.Value % 60);

        //formats the minutes and seconds to display as 00:00
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [ClientRpc]
    private void UpdateClienttimer222ClientRpc()
    {
        elapsedTime.Value = 0;
        timerDisplay.text = "00:00";
        Debug.Log("UpdateClienttimer222ClientRpc run");
    }
    */

}