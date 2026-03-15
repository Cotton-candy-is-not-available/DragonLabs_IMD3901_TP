using UnityEngine;
using UnityEngine.UI;

public class PauseGameController : MonoBehaviour
{
    public Button pause_button;
    public Button resume_button;

    //drag and drop your game's timer into this variable in the inspector
    public GameObject gameTimer;

    public void pauseGame(GameObject timer)
    {
        Debug.Log("paused game");
        //hide the pause button and show the resume button
        pause_button.gameObject.SetActive(false);
        resume_button.gameObject.SetActive(true);

        //pause the game's timer

    }

    public void resumeGame(GameObject timer)
    {
        Debug.Log("resumed game");
        //hide the resume button and show the pause button
        pause_button.gameObject.SetActive(true);
        resume_button.gameObject.SetActive(false);

        //resume the game's timer


    }


}
