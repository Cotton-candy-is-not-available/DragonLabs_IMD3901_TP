using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI timerDisplay;
    float elapsedTime = 90.0f;
    public PiñataController piñataController_access;

    void Update()
    {
        if (piñataController_access.isGameOver == false) //if the game is not over the timer should be counting
        {
            elapsedTime -= Time.deltaTime; //calculates all of the time passed since game started

            if (elapsedTime <= 0)
            {
                elapsedTime = 0;
                piñataController_access.isGameOver = true;
            }

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            //formats the minutes and seconds to display as 00:00
            timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else //if the game is over (either failure or victory), reset the timer
        {
            elapsedTime = 0;
            timerDisplay.text = "00:00";
        }
    }
}