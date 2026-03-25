using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static Unity.Burst.Intrinsics.X86.Avx;

public class startBlurEffect : NetworkBehaviour
{
    public GameObject cup;
    public GameObject drinkMeSign;
    public XRGrabInteractable cupVRgrabInteractable;
    public gameManager gameManager;

    public bool Player1Drink;
    public bool Player2Drink;

    public NetworkVariable<bool> activate;

    public override void OnNetworkDespawn()
    {
        activate.Value = false;
    }
    void Start()
    {
        //find the players 1 and 2 by tag
        //get their post processing volumes

        //cupVRgrabInteractable.enabled = false;//don't allow VR player to grab cup by default

        //neither player 1 or 2 are drinking
        Player1Drink = false;
        Player2Drink = false;

        drinkMeSign.SetActive(activate.Value);//disactivate drink me sign by default

    }

    public void Update()
    {
        drinkMeSign.SetActive(activate.Value);//activate drink me sign so player knows which cup to drink from

    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "ball(Clone)")//if the ball touches the beer 
        {
            Debug.Log("touched beer");
            //cup.tag = "Interactable";//change cup to interactable to player can drink
            //cupVRgrabInteractable.enabled = true;//allow VR player to grab cup 

            activate.Value = true;
            //drinkMeSign.SetActive(true);//activate drink me sign so player knows which cup to drink from

            if (gameObject.tag == "cup1")
            {
                //add point to player 2
                gameManager.player2Points.Value += 1;//tell game manager to give a point to player 2


                Debug.Log("player 1 cup ");

            }
            else if(gameObject.tag == "cup2")
            {
                //add point to player 1
                gameManager.player1Points.Value += 1;//tell game manager to give a point to player 1

                Debug.Log("player 2 cup ");

            }
        }

    }


}
