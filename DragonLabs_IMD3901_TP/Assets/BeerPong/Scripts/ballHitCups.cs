using TMPro;
using UnityEngine;

public class ballHitCups : MonoBehaviour
{
    public bool P1Point = false;
    public bool P2Point = false;

    public bool nonCup = false;

    private void OnCollisionEnter(Collision collision)
    {
        //change cup and cup 2 tag to P1Cup and P2Cup
        if (collision.gameObject.tag == "cup")//if the ball hits a cup
        {
            P2Point = true;//tell game manager to give a point to player 2

        }

        else if (collision.gameObject.tag == "cup2")
        {
            P1Point = true;//tell game manager to give a point to player 1

        }

        else if (collision.gameObject.tag == "floor")
        {
            nonCup = true;//to be used when it is thrown and has not hit any beer/cups so it needs to reset
        }




    }




}
