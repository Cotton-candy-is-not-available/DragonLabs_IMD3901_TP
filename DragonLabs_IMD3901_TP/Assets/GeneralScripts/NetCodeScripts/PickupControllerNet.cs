using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PickupControllerNet : NetworkBehaviour
{
    [SerializeField] private Transform holdArea;

    public GameObject heldObj;
    public GameObject HeldObject => heldObj;

    private Rigidbody heldObjRB;

    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private Scene currentScene;

    public float throwForce = 10f;
    [SerializeField] private tragectoryLine line;
    public float mass = 10;

    public NetworkVariable<bool> enableLine = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        enableLine.Value = false;
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (!IsOwner) return;

        // If a tic tac toe piece got placed, stop controlling it immediately
        if (heldObj != null)
        {
            PieceNet piece = heldObj.GetComponent<PieceNet>();
            if (piece != null && piece.IsPlaced)
            {
                ForceClearHeldObject();
                return;
            }
        }

        // PICK UP
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            Debug.Log("i was presssed to pickup object");

            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    pickupObject(hit.transform.gameObject);
                }
            }
        }

        // DROP
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame && heldObj != null)
        {
            PieceNet piece = heldObj.GetComponent<PieceNet>();
            if (piece == null || !piece.IsPlaced)
            {
                Debug.Log("tab was presssed to drop object");
                dropObject();
            }
        }

        // MOVE HELD OBJECT
        if (heldObj != null)
        {
            PieceNet piece = heldObj.GetComponent<PieceNet>();
            if (piece == null || !piece.IsPlaced)
            {
                moveObject();
            }
        }

        // Beer pong line
        if (currentScene.name == "beerPong")
        {
            if (heldObj != null && heldObj.name == "ball(Clone)")
            {
                enableLine.Value = true;
                line.drawTragectory(transform.forward * throwForce, enableLine);
            }
            else
            {
                enableLine.Value = false;
                line.drawTragectory(transform.forward * throwForce, enableLine);
            }
        }
    }

    void pickupObject(GameObject pickObj)
    {
        if (!IsOwner) return;

        NetworkObject netObj = pickObj.GetComponent<NetworkObject>();
        if (netObj == null) return;

        PickupObjectServerRpc(netObj.NetworkObjectId, OwnerClientId);
    }

    void dropObject()
    {
        if (!IsOwner) return;
        if (heldObj == null) return;

        NetworkObject netObj = heldObj.GetComponent<NetworkObject>();
        if (netObj != null)
            DropObjectServerRpc(netObj.NetworkObjectId);

        heldObj = null;
        heldObjRB = null;
    }

    void moveObject()
    {
        if (heldObj == null) return;

        heldObj.transform.position = holdArea.position;
        heldObj.transform.rotation = holdArea.rotation;
    }

    public void ForceClearHeldObject()
    {
        if (heldObj == null) return;

        heldObj.transform.SetParent(null, true);

        PieceNet piece = heldObj.GetComponent<PieceNet>();
        Rigidbody rb = heldObj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            if (piece != null && piece.IsPlaced)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
            else
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        heldObj = null;
        heldObjRB = null;
    }

    [ServerRpc(RequireOwnership = false)]
    void PickupObjectServerRpc(ulong objectId, ulong playerClientId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(objectId)) return;

        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
        netObj.ChangeOwnership(playerClientId);

        NetworkObject playerObj = NetworkManager.Singleton.ConnectedClients[playerClientId].PlayerObject;
        if (playerObj == null) return;

        PickupControllerNet pickup = playerObj.GetComponentInChildren<PickupControllerNet>();
        if (pickup == null || pickup.holdArea == null) return;

        netObj.transform.SetParent(pickup.holdArea);

        AssignHeldObjectClientRpc(netObj.NetworkObjectId);
    }

    [ServerRpc(RequireOwnership = false)]
    void DropObjectServerRpc(ulong objectId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(objectId)) return;

        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
        GameObject obj = netObj.gameObject;

        PieceNet piece = obj.GetComponent<PieceNet>();

        netObj.transform.SetParent(null);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (piece != null && piece.IsPlaced)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
            else
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;

                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    if (currentScene.name == "beerPong")
                    {
                        Debug.Log("beerpong scene");
                        rb.linearVelocity = transform.forward * throwForce;
                    }
                }
            }
        }

        ClearHeldObjectClientRpc();
    }

    [ClientRpc]
    void AssignHeldObjectClientRpc(ulong objectId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(objectId)) return;

        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
        heldObj = netObj.gameObject;
        heldObjRB = heldObj.GetComponent<Rigidbody>();

        PieceNet piece = heldObj.GetComponent<PieceNet>();
        if (piece != null && piece.IsPlaced)
        {
            ForceClearHeldObject();
            return;
        }

        heldObj.transform.position = holdArea.position;
        heldObj.transform.rotation = holdArea.rotation;

        if (heldObjRB != null)
        {
            heldObjRB.useGravity = false;
            heldObjRB.isKinematic = true;
        }
    }

    [ClientRpc]
    void ClearHeldObjectClientRpc()
    {
        heldObj = null;
        heldObjRB = null;
    }
}