using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.UI;

public class TimerControllerNet : NetworkBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI timerDisplay;

    public NetworkVariable<float> elapsedTime;
    public NetworkVariable<float> elapsedCandyTime;

    public PinataControllerNet pinataControllerNet_access;
    public NetworkVariable<bool> isExtraTimeDone; //for collecting candy

    public Image timerBGimage;

    public override void OnNetworkSpawn()
    {
        //set initial values
        elapsedTime.Value = 90.0f;
        elapsedCandyTime.Value = 25.0f;
    }

    private void Update()
    {
        if (!IsServer) //only the server should be updating the elapsed time and doing the math
        {
            return;
        }
        UpdateTimerServerRpc(); //update the client and host's display
    }


    [ServerRpc(RequireOwnership = false)]
    private void UpdateTimerServerRpc()
    {
        //Debug.Log("UpdateTimerServerRpc run");

        if (pinataControllerNet_access.isGameOver.Value == false) //if the game is not over the timer should be counting
        {
            elapsedTime.Value -= Time.deltaTime; //calculates all of the time passed since game started

            if (elapsedTime.Value <= 0)
            {
                elapsedTime.Value = 0;
                pinataControllerNet_access.isGameOver.Value = true;
            }

            int minutes = Mathf.FloorToInt(elapsedTime.Value / 60);
            int seconds = Mathf.FloorToInt(elapsedTime.Value % 60);

            //formats the minutes and seconds to display as 00:00
            timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            UpdateTimerClientRpc(); //update the client
        }
        else //if the game is over (either failure or victory), reset the timer
        {
            elapsedTime.Value = 0;
            timerDisplay.text = "00:00";
            pinataControllerNet_access.isGameOver.Value = true;
            UpdateTimerClientRpc(); //update the client
            startExtraCandyTimeServerRpc(); //start extra candy timer for host and client
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

    [ServerRpc(RequireOwnership = false)]
    public void startExtraCandyTimeServerRpc()
    {
        //change the colour of the timer background to orange to indicate change
        timerBGimage.GetComponent<Image>().color = new Color32(255, 162, 0, 255);

        elapsedCandyTime.Value -= Time.deltaTime; //calculates all of the time passed since extra time started

        if (elapsedCandyTime.Value <= 0)
        {
            elapsedCandyTime.Value = 0;
            isExtraTimeDone.Value = true;
        }

        int minutes = Mathf.FloorToInt(elapsedCandyTime.Value / 60);
        int seconds = Mathf.FloorToInt(elapsedCandyTime.Value % 60);

        //formats the minutes and seconds to display as 00:00
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        updateCandyTimeClientRpc();
    }

    [ClientRpc]
    public void updateCandyTimeClientRpc()
    {
        timerBGimage.GetComponent<Image>().color = new Color32(255, 162, 0, 255);

        int minutes = Mathf.FloorToInt(elapsedCandyTime.Value / 60);
        int seconds = Mathf.FloorToInt(elapsedCandyTime.Value % 60);

        //formats the minutes and seconds to display as 00:00
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}