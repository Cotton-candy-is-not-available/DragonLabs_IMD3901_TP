using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static Unity.Burst.Intrinsics.X86.Avx;

public class startBlurEffect : MonoBehaviour
{
    public GameObject cup;
    public GameObject drinkMeSign;
    public XRGrabInteractable cupVRgrabInteractable;
    //might need to make network variable for drink me sign
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find the players 1 and 2 by tag
        //get their post processing volumes

        //cupVRgrabInteractable.enabled = false;//don't allow VR player to grab cup by default

        drinkMeSign.SetActive(false);//disactivate drink me sign by default

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "ball(Clone)")//if the ball touches the beer 
        {
            Debug.Log("touched beer");
            //cup.tag = "Interactable";//change cup to interactable to player can drink
            //cupVRgrabInteractable.enabled = true;//allow VR player to grab cup 

            drinkMeSign.SetActive(true);//activate drink me sign so player knows which cup to drink from

            if (gameObject.tag == "cup1")
            {
                //add point to player 2
                //increase player 1's blur
                Debug.Log("player 1 cup ");

            }
            else if(gameObject.tag == "cup2")
            {
                //add point to player 1
                //increase player 2's blue

                Debug.Log("player 2 cup ");

            }
        }

    }


}
