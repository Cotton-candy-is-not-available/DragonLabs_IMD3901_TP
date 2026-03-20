using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(NetworkObject))]

public class ObjectGrabIneractableNet : NetworkBehaviour
{
    private XRGrabInteractable grabInteractable;
    private NetworkObject netObj;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        netObj = GetComponent<NetworkObject>();
    }
    private void OnEnable()
    {
        //when an object is picked up start running the OnSelectGrabbable function
        grabInteractable.selectEntered.AddListener(OnSelectGrabbable);
    }

    private void OnDisable()
    {
        //when an object is dropped stop running the OnSelectGrabbable function
        grabInteractable.selectEntered.RemoveListener(OnSelectGrabbable);
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        Debug.Log("a GRAB was detected by a raycast");

        if (IsOwner) //if its the host trying to pick up the object
        {
            return; //do nothing because host already has ownership
        }

        if(IsClient) //if its the client trying to pick up the object
        {
            //retrieve the network object thats attached to the client's raycast
            //get the transform of the network object
            NetworkObject networkObjSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();

            if (networkObjSelected != null) //if something is picked up
            {
                //send request to the server to give the client ownership to move the object around
                RequestGrabbableOwnershipServerRpc(networkObjSelected.NetworkObjectId);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]                       //rpc parameters is same as fetching the playerId uLong
    public void RequestGrabbableOwnershipServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        //retrieve the object's network object from the server's record of spawned objects
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];

        if (netObj.TryGetComponent<NetworkObject>(out netObj)) {
            //transfer ownership so the client can interact with it (playerId here is SenderClientId)
            netObj.ChangeOwnership(rpcParams.Receive.SenderClientId); //give client ownership access
            Debug.Log("ownership given to client");
        }
        
    }

}
