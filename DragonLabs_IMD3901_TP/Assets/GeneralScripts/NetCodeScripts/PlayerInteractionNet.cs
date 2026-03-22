using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerInteractionNet : NetworkBehaviour
{
    public float interactRange = 5f;
    public Camera playerCamera;

    public Crosshair crosshair_access;
    public PickupController pickupControllerNet_access;

    Scene currentScene;

    //temp for dubugging
    public TextMeshProUGUI debugText;
   
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();//get the current scene

        //check to see if its the HOST
        if (!IsOwner)
        {
            return;
        }

        //creating a ray that shoots out of the camera to detect objects in front of us
        //we want the ray to be in front of us
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit; //hit box for the ray cast

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (IsOwner)
                {
                    //checking if the ray hits something with a collider that is interactable
                    crosshair_access.setInteractServerRpc(true);
                }
                if (Keyboard.current.iKey.wasPressedThisFrame)
                {
                    if (IsHost)
                    {
                        Debug.Log("HOST pressed I");
                    }
                    else if (IsClient)
                    {
                        Debug.Log("CLIENT pressed I");
                        sendHoldRequestToServerRpc();
                    }
                }

                if (Keyboard.current.pKey.wasPressedThisFrame)
                {
                    Debug.Log("object pressed was: " + hit.collider.gameObject.name);

                    minigameButtonAnimate button = hit.collider.GetComponent<minigameButtonAnimate>();

                    if (button != null)
                    {
                        if ((int)OwnerClientId  == 0) //host
                        {
                            button.animateButton();
                            button.switchSceneOnButtonServerRpc();
                        }

                        if ((int)OwnerClientId  == 1) //client
                        {
                            button.PressButtonServerRpc(button.NetworkObjectId);
                            button.switchSceneOnButtonServerRpc();
                        }
                    }

                    //Debug.Log("interact was set to true");
                    return;
                }

                if (currentScene.name == "beerPong"){//only enable in beerPong scene

                    if (Keyboard.current.rKey.isPressed)//if r key was held down
                    {
                        if (hit.collider.gameObject.GetComponent<pourDetector>() !=null)
                        {
                            NetworkObject cupNetObj = hit.collider.gameObject.GetComponent<NetworkObject>();
                            cupNetObj.GetComponent<pourDetector>().rotateCupServerRpc(cupNetObj.NetworkObjectId);
                            debugText.text = "ROTATE";

                        }
                        else//if it doesnt have the pour detector script do nothing and go back
                        {
                            debugText.text = "can't rotate";
                            return;
                        }
                    }
                }

            }
        }
        if (IsOwner)
        {
            crosshair_access.setInteractServerRpc(false); //set it back to false if we look away from the object
        }
    }

    //when server, debug appeared on HOST console
    //when owner, debug appeared on CLIENT console
    //when ClientsAndHost, debug appeared on BOTH consoles
    [Rpc(SendTo.Server)]
    public void sendHoldRequestToServerRpc()
    {
        Debug.Log("request to SERVER sent");
    }
}
