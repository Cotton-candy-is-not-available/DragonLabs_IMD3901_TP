using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class pourDetector : NetworkBehaviour
{
    public int pourThreshold = 45;

    public Transform origin = null;

    public bool isPouring = false;

    public float fillLevel;

    Renderer rend;

    public GameObject beerLiquid;//beer liquid
    float time = 0.5f;


    // ------ PC variables --------------
    [SerializeField] float rotationProgress;
    [SerializeField] Quaternion PCStartRotation;
    [SerializeField] Quaternion PCEndRotation;

    [SerializeField] Quaternion cupRotation;

    // ------ VR variables --------------

    [SerializeField] float VRPourRotation = 30.0f;


    public float pressDistance = 0.3f;
    public float pressSpeed = 2f;

    public NetworkObject cupNetObj;


    public PickupControllerNet PickupControllerNet;

    public GameObject held;

    //public Transform holdArea;


    private void Start()
    {
        rend = beerLiquid.GetComponent<Renderer>();//get the renderer from the gameobject
        PCStartRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//default rotation

        PCEndRotation = Quaternion.Euler(-90.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//rotates 90degrees towards player


    }




    public void lowerFillLevel()
    {
        //lower fill level
        time += Time.deltaTime;

        //set fill level variable from script to be original fill level then chnage it
        //otherwise it is always 0
        // decrease fill level over time
        //fillLevel = Mathf.Lerp(fillLevel, 0, Time.deltaTime);

        //send over to shader new value of fill level
        rend.material.SetFloat("_fillLevel", fillLevel);//reference names in shader graph
        Debug.Log("fillLevel: " + fillLevel);
        Debug.Log("fill down");

    }

    public void rotateCup()
    {
        rotationProgress += Time.deltaTime * 5;//somewhat slowly rotate; Note: smaller number slower, bigger number faster
        gameObject.transform.rotation = Quaternion.Lerp(PCStartRotation, PCEndRotation, rotationProgress);//rotates watering can smoothly
        //lowerFillLevel();//lower the liquid inside the cup

        //StartCoroutine(destroyCup(PickupControllerNet.heldObj.GetComponent<NetworkObject>()));//destoy the cup
    }



    [ServerRpc(RequireOwnership = false)]
    public void rotateCupServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (cupNetObj.TryGetComponent<NetworkObject>(out cupNetObj))
        {
            cupNetObj.ChangeOwnership(rpcParams.Receive.SenderClientId);

            rotateCupClientRpc();
        }
    }


    [ClientRpc]
    private void rotateCupClientRpc()
    {
        if (IsOwner)
        {
            rotateCup();
        }
    }









    IEnumerator destroyCup(NetworkObject heldObj)
    {
        //play poof soundFX
        //show poof effect(particles?)
        heldObj = heldObj.GetComponent<NetworkObject>();//instatiate the object
        yield return new WaitForSeconds(3); //waits 3 seconds
        heldObj.Despawn();
        //Destroy(heldObj); //destroy the cup
    }


    //For VR
    //If gameobject.rotation.x < 90//being  poured/rotated
    //lowerFillLevel
    //destroy gameobject


}
