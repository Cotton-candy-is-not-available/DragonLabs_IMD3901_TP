using System;
using Unity.Netcode;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class activatePouring : NetworkBehaviour
{
    //wil be added to player when in  beer pong scene
    Scene currentScene;

    pourDetector pouring;

    public PickupControllerNet PickupControllerNet;

    public GameObject held;

    public Transform holdArea;

    private void Start()
    {
        // Get current scene name
        currentScene = SceneManager.GetActiveScene();
    }
    // Update is called once per frame
    void Update()
    {
        //held = PickupControllerNet.heldObj;

        //if (currentScene.name == "beerPong"){//only enable in beerPong scene

        //if (PickupControllerNet.heldObj != null && PickupControllerNet.heldObj.layer == 7 || PickupControllerNet.heldObj != null && PickupControllerNet.heldObj.layer == 8)//if the player is holding an object and that object is on the P1Cup of P2Cup layers
            if (PickupControllerNet.heldObj != null )
        {
            Debug.Log("not null");

                if (Keyboard.current.pKey.isPressed) //if p is pressed to drink
                {
                    Debug.Log("p key was pressed net pouring");
                    //pouring = PickupControllerNet.heldObj.GetComponent<pourDetector>();//get the cups pour detector script
                    holdArea.rotation = Quaternion.Euler(90f, 0f, 0f);
        //transform.rotation = Quaternion.Lerp(PCStartRotation, PCEndRotation, rotationProgress);//rotates watering can smoothly

                //pouring.startPouringServerRpc();
                //pouring.startPouringServerRpc(PickupControllerNet.heldObj.GetComponent<NetworkObject>().NetworkObjectId);
            }
            else
                {
                    pouring = PickupControllerNet.heldObj.GetComponent<pourDetector>();//get the cups pour detector script
                    pouring.StopPouring();//stop pouring
                    Debug.Log("else STOP");

                }
        }
        //}
    }
}
