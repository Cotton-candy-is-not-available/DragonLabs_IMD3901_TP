using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class pourDetector : NetworkBehaviour
{

    public float fillLevel;

    Renderer rend;

    public GameObject beerLiquid;//beer liquid


    // ------ PC variables --------------
    [SerializeField] float rotationProgress;
    [SerializeField] Quaternion PCStartRotation;
    [SerializeField] Quaternion PCEndRotation;

    // ------ VR variables --------------

    [SerializeField] float VRPourRotation = 30.0f;


    public float pressDistance = 0.3f;
    public float pressSpeed = 2f;

    public NetworkObject cupNetObj;


    float fillElaspsedTime;
    float lerpDuration = 3;


    private void Start()
    {
        rend = beerLiquid.GetComponent<Renderer>();//get the renderer from the gameobject
        fillLevel = 0.7f;//set fill level
        rend.material.SetFloat("_fillLevel", fillLevel);//reference names in shader graph so that it matches the fill level in this script


        PCStartRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//default rotation

        PCEndRotation = Quaternion.Euler(-180.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//rotates 90degrees towards player

    }




    public void lowerFillLevel()
    {
        //lower fill level
          
                // decrease fill level over time
                fillLevel = Mathf.Lerp(fillLevel, 0.01f, fillElaspsedTime/lerpDuration);

            //send over to shader new value of fill level
            rend.material.SetFloat("_fillLevel", fillLevel);//reference names in shader graph
            Debug.Log("fillLevel: " + fillLevel);
            Debug.Log("fill down");

            fillElaspsedTime += Time.deltaTime;

        
      
    }

    public void rotateCup()
    {

            transform.rotation = Quaternion.Lerp(PCStartRotation, PCEndRotation, rotationProgress/lerpDuration);//rotates watering can smoothly
            rotationProgress += Time.deltaTime * 7;//slowly rotate

            lowerFillLevel();

            StartCoroutine(destroyCup(cupNetObj));//destoy the cup

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









    IEnumerator destroyCup(NetworkObject cupNetObj)
    {
        //play poof soundFX
        //show poof effect(particles?)
        cupNetObj = cupNetObj.GetComponent<NetworkObject>();
        yield return new WaitForSeconds(3); //waits 3 seconds
        cupNetObj.Despawn();
        //Destroy(heldObj); //destroy the cup
    }


    //For VR
    //If gameobject.rotation.x < 90//being  poured/rotated
    //lowerFillLevel
    //destroy gameobject


}
