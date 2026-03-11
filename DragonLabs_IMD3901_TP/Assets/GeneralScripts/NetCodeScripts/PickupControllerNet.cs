using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupControllerNet : NetworkBehaviour
{
    [SerializeField] Transform holdArea; //will be parented to this

    //the object that is picked up
    private GameObject heldObj;
    public GameObject HeldObject => heldObj;
    private Rigidbody heldObjRB;

    //physics
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update()
    {
        //PICKING UP-----------------------------
        if (Keyboard.current.iKey.wasPressedThisFrame) //if i was pressed to pick up
        {
            Debug.Log("i was presssed to pickup object");

            if (heldObj == null) //if an object is NOT already being held
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    //pick up the object
                    pickupObject(hit.transform.gameObject);
                }
            }
        }

        //DROPPING-----------------------------
        if (Keyboard.current.tabKey.wasPressedThisFrame && heldObj != null) //if tab was pressed to drop
        {
            Debug.Log("tab was presssed to drop object");
            dropObject();
        }

        //MOVING-----------------------------
        if (heldObj != null) //if an object is currently being held
        {
            //move the object around
            moveObject();
        }
    }

    /*----------------FUNCTIONS---------------*/
    void pickupObject(GameObject pickObj)
    {
        if (!IsOwner) return; //only the player controlling this can request pickup

        NetworkObject netObj = pickObj.GetComponent<NetworkObject>(); //get the network object of the pickObj
        if (netObj == null) return;
        PickupObjectServerRpc(netObj.NetworkObjectId, OwnerClientId); //ask server to pick it up with RPC
    }

    void dropObject()
    {
        if (!IsOwner) return;
        if (heldObj == null) return;

        NetworkObject netObj = heldObj.GetComponent<NetworkObject>(); //get network object of heldObj
        if (netObj != null)
            DropObjectServerRpc(netObj.NetworkObjectId);

        //clear local reference
        heldObj = null;
        heldObjRB = null;
    }

    void moveObject()
    {
        if (heldObj == null) return;
        //snap object instantly to hold area
        heldObj.transform.position = holdArea.position;
        heldObj.transform.rotation = holdArea.rotation;
    }

    [ServerRpc(RequireOwnership = false)]
    void PickupObjectServerRpc(ulong objectId, ulong playerClientId)
    {
        //retrieve the object's network object from the server's record of spawned objects
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];

        //transfer ownership so the client can interact with it
        netObj.ChangeOwnership(playerClientId); //give client ownership access

        //parent the object to the player's hold area
        netObj.transform.SetParent(NetworkManager.Singleton.ConnectedClients[playerClientId].PlayerObject.transform);

        AssignHeldObjectClientRpc(netObj.NetworkObjectId);
    }

    [ServerRpc(RequireOwnership = false)]
    void DropObjectServerRpc(ulong objectId)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];

        //unparent the object
        netObj.transform.SetParent(null);

        Rigidbody rb = netObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //clear the rigidbody's attributes
            rb.useGravity = true; //enable gravity again
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; 
            rb.constraints = RigidbodyConstraints.None; //allow full movement
        }
        ClearHeldObjectClientRpc();
    }

    [ClientRpc]
    void AssignHeldObjectClientRpc(ulong objectId)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
        heldObj = netObj.gameObject;
        heldObjRB = heldObj.GetComponent<Rigidbody>();

        //snap object instantly to hold area
        heldObj.transform.position = holdArea.position;
        heldObj.transform.rotation = holdArea.rotation;

        if (heldObjRB != null)
        {
            heldObjRB.useGravity = false; //turn gravity off so it floats in the air
        }
    }

    [ClientRpc]
    void ClearHeldObjectClientRpc()
    {
        //reset the held object from the client
        heldObj = null;
        heldObjRB = null;
    }

}
