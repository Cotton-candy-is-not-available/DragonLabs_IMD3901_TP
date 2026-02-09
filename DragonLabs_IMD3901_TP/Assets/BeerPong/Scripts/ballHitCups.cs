using TMPro;
using UnityEngine;

public class ballHitCups : MonoBehaviour
{
    public TMP_Text player1PointsText;
    public TMP_Text player2PointsText;

    public int player1Points = 0;
    public int player2Points = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "cup")//if the ball hits a cup
        {
            //for alpha test; disable the ball and make it re-appear infront of player 

            gameObject.SetActive(false);//turn off
            player2Points+=1;//increase points
            player2PointsText.text = ("Them: "+ player2Points);//update text
            Debug.Log("player2Points: "+ player2Points);


            gameObject.transform.position = new Vector3(0.0109999999f, 1.60599995f, -6.93300009f);//go back to be infront of player
            gameObject.SetActive(true);//make appear again
        }

        else if (collision.gameObject.tag == "cup2")
        {
            gameObject.SetActive(false);//turn off
            player1Points+=1;//increase points
            player1PointsText.text = ("You: "+ player1Points);//update text
            Debug.Log("player1Points: "+ player1Points);

            gameObject.transform.position = new Vector3(0.0109999999f, 1.60599995f, -6.93300009f);//go back to be infront of player
            gameObject.SetActive(true);//make appear again
        }







            //for later
            //destroy ball and call a function to check how many balls are in the scene after it is destroyed or null
            //check who's turn it is
            //instatiate ball infront of player's turn

            //OR
            //ball manager
            //instatiate ball on turn
            //if instatiated ball == null
            //re instatiate infront of next turn
        
    }




}
