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
    public bool table = false;

    private void OnCollisionEnter(Collision collision)
    {
      

        if (collision.gameObject.tag == "floor")
        {
            nonCup = true;//to be used when it is thrown and has not hit any beer/cups so it needs to reset
        }

        else if (collision.gameObject.tag == "table")
        {
            table = true;//to be used when it is thrown and has not hit any beer/cups so it needs to reset
        }
        else if (collision.gameObject.tag != "table")
        {
            table = false;//to be used when it is thrown and has not hit any beer/cups so it needs to reset

        }



    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.tag == "cup1")//if the ball hits player 1 Cup 
        {
            P2Point = true;//tell game manager to give a point to player 2

        }

        else if (trigger.gameObject.tag == "cup2")//player 2 cup 
        {
            P1Point = true;//tell game manager to give a point to player 1

        }
    }


    private void Update()
    {
        if(gameObject.transform.position.y < 0)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();//destroy ball when it goes anywhere below floor level
        }
    }



}
