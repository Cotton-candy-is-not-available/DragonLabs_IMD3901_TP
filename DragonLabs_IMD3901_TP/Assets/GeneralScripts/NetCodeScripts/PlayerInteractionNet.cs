using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionNet : NetworkBehaviour
{
    public float interactRange = 5f;
    public Camera playerCamera;

    public Crosshair crosshair_access;
    public PickupController pickupControllerNet_access;
   
    void Update()
    {
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
                //checking if the ray hits something with a collider that is interactable
                crosshair_access.setInteractServerRpc(true);

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

                //Debug.Log("interact was set to true");
                return;
            }
        }
        crosshair_access.setInteractServerRpc(false); //set it back to false if we look away from the object
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
