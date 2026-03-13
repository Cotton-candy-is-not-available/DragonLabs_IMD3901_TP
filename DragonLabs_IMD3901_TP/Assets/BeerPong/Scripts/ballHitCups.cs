using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ballHitCups : NetworkBehaviour
{
    //public NetworkVariable<bool> P1Point = new NetworkVariable<bool>();
    //public NetworkVariable<bool> P2Point = new NetworkVariable<bool>();

    public bool P1Point = false;
    public bool P2Point = false;

    public bool nonCup = false;

    private void OnCollisionEnter(Collision collision)
    {
        //change cup and cup 2 tag to P1Cup and P2Cup
        if (collision.gameObject.layer == 7)//if the ball hits player 1 Cup 
        {
            P2Point = true;//tell game manager to give a point to player 2

        }

        else if (collision.gameObject.layer == 8)//player 2 cup 
        {
            P1Point = true;//tell game manager to give a point to player 1

        }

        else if (collision.gameObject.tag == "floor")
        {
            nonCup = true;//to be used when it is thrown and has not hit any beer/cups so it needs to reset
        }




    }




}
