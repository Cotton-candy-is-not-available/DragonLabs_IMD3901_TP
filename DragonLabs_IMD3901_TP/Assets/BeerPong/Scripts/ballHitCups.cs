using TMPro;
using UnityEngine;

public class ballHitCups : MonoBehaviour
{
    //public TMP_Text player1PointsText;
    //public TMP_Text player2PointsText;

    //public int player1Points = 0;
    //public int player2Points = 0;

    public bool P1Point = false;
    public bool P2Point = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "cup")//if the ball hits a cup
        {
            P2Point = true;//tell game manager to give a point to player 2

        }

        else if (collision.gameObject.tag == "cup2")
        {
            P1Point = true;//tell game manager to give a point to player 1

        }




    }




}
