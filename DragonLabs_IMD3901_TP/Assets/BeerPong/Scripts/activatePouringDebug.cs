using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class activatePouringDebug : MonoBehaviour
{
    //wil be added to player when in  beer pong scene
    Scene currentScene;

    pourDetector pouring;

    public PickupController PickupController;

    public GameObject held;

    private void Start()
    {
        // Get current scene name
        currentScene = SceneManager.GetActiveScene();
    }
    // Update is called once per frame
    void Update()
    {
        held = PickupController.heldObj;

        //if (currentScene.name == "beerPong"){//only enable in beerPong scene

        //if (PickupControllerNet.heldObj != null && PickupControllerNet.heldObj.layer == 7 || PickupControllerNet.heldObj != null && PickupControllerNet.heldObj.layer == 8)//if the player is holding an object and that object is on the P1Cup of P2Cup layers
            if (PickupController.heldObj != null )
        {
            Debug.Log("not null");

                if (Keyboard.current.pKey.isPressed) //if p is pressed to drink
                {
                    Debug.Log("p key was pressed");
                    pouring = PickupController.heldObj.GetComponent<pourDetector>();//get the cups pour detector script
                    pouring.startPouringDebug();//start pouring
                }
                else
                {
                    pouring = PickupController.heldObj.GetComponent<pourDetector>();//get the cups pour detector script
                    pouring.StopPouring();//stop pouring
                    Debug.Log("else STOP");

                }
        }
        //}
    }
}
