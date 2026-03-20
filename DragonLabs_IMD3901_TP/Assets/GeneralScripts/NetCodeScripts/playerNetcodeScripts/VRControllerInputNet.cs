using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRControllerInputNet : NetworkBehaviour
{
    //use a network variable boolean so that host and client can synch the states of the variable
    public NetworkVariable<bool> isTriggerPressed = new NetworkVariable<bool>(false);

    //access the right hand ray interactors of both host and client 
    private XRRayInteractor rightHandRay;

    //public AudioManager audioManager_access;

    void Start()
    {
        if (!IsOwner) return; //only get the local player's hand

        //access the right hand rays
        rightHandRay = GetComponentInChildren<XRRayInteractor>();
        
    }


    void Update()
    {
        if (!IsOwner) return;

        //get list of right hand devices
        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count == 1)
        {
            UnityEngine.XR.InputDevice device = rightHandDevices[0];

            bool triggerValue;
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Debug.Log("Trigger button is pressed.");

                //check to see if the ray is colliding with something (like in pc)
                if (rightHandRay.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    //check if the object has the tag "Interactable"
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        Debug.Log("Trigger pressed! Ray hit: " + hit.collider.name);

                        string buttonName = hit.collider.gameObject.name;
                        //animate only the tile that was pressed/looked at
                        //TileAnimate tile = hit.collider.GetComponent<TileAnimate>();
                        {

                            if (IsServer)
                            {
                                //if host, directly animate the tile
                                //tile.AnimateTile();
                                Debug.Log("HOST PRESSED BUTTON.");

                            }
                            else if (IsClient)
                            {
                                //if client, request for ownership, animate the tile and synch
                                //tile.PressTileServerRpc(tile.NetworkObjectId);
                                //play the sound and keep track of which tile player2 played
                                Debug.Log("C:IENT PRESSED BUTTON.");

                            }
                        }
                    }
                }




            }
        }


        //if (Keyboard.current.tKey.wasPressedThisFrame) //for mock HMD
       // {
            //check to see if the ray is colliding with something (like in pc)
           /* if (rightHandRay.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                //check if the object has the tag "Interactable"
                if (hit.collider.CompareTag("Interactable"))
                {
                    Debug.Log("Trigger pressed! Ray hit: " + hit.collider.name);

                    string buttonName = hit.collider.gameObject.name;
                    //animate only the tile that was pressed/looked at
                    //TileAnimate tile = hit.collider.GetComponent<TileAnimate>();
                    {

                        if (IsServer)
                        {
                            //if host, directly animate the tile
                            //tile.AnimateTile();
                        }
                        else if (IsClient)
                        {
                            //if client, request for ownership, animate the tile and synch
                            //tile.PressTileServerRpc(tile.NetworkObjectId);
                            //play the sound and keep track of which tile player2 played
                        }





                    }
                }
            }*/
        //}
    }

    //------------------------------------------------------------------------------
        //VR input with right hand ray - this is for the real controller trigger value
        //for this project i have decidied to use the T key for mock HMD instead
        //var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        //UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);

        /*if (rightHandDevices.Count == 1)
        {
            UnityEngine.XR.InputDevice device = rightHandDevices[0];

            bool triggerValue;
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Debug.Log("Trigger button is pressed.");
            }
        }*/
        //----------------------------------------------------------------------------









}