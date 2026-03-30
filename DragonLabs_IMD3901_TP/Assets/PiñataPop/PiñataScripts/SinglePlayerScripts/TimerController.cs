using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI timerDisplay;

    float elapsedTime = 90.0f;
    float elapsedCandyTime = 25.0f;

    public PinataController pinataController_access;
    public ChooseGame chooseGame_access;
    public bool isExtraTimeDone; //for collecting candy

    public Image timerBGimage;

    private void Start()
    {
        //set initial values
        elapsedTime = 90.0f;
        elapsedCandyTime = 25.0f;
        isExtraTimeDone = false;
    }

    void Update()
    {
        if (pinataController_access.isGameOver == false) //if the game is not over the timer should be counting
        {
            elapsedTime -= Time.deltaTime; //calculates all of the time passed since game started

            if (elapsedTime <= 0)
            {
                elapsedTime = 0;
                pinataController_access.isGameOver = true;
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
            pinataController_access.isGameOver = true;
            startExtraCandyTime(); //25 seconds for collecting candy points
        }
    }

    public void startExtraCandyTime()
    {
        //Debug.Log("started extra candy time!");

        //change the colour of the timer background to orange to indicate change
        timerBGimage.GetComponent<Image>().color = new Color32(255, 162, 0, 255);

        elapsedCandyTime -= Time.deltaTime; //calculates all of the time passed since extra time started

        if (elapsedCandyTime <= 0)
        {
            elapsedCandyTime = 0;
            isExtraTimeDone = true;
            pinataController_access.isGameOver = true;
            StartCoroutine(waitToSwitch());
        }

        int minutes = Mathf.FloorToInt(elapsedCandyTime / 60);
        int seconds = Mathf.FloorToInt(elapsedCandyTime % 60);

        //formats the minutes and seconds to display as 00:00
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator waitToSwitch()
    {
        Debug.Log("called waitToSwitch");
        //yield on a new YieldInstruction that waits for 15 seconds.
        yield return new WaitForSeconds(15);

        //change scenes back to the lobby
        chooseGame_access.switchScenes("Lobby");
    }

}