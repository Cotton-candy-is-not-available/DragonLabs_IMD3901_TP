using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class pourDetector : NetworkBehaviour
{
    //this script is attached to the cup itself
    public Vector3 fillLevel = new Vector3(0f, -0.23f, 0f);

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

    public GameObject cupNetObj;


    float fillElaspsedTime;
    float lerpDuration = 3;

    public gameManager gameManager;
    public DepthOfField blurEffect;


    private void Start()
    {
        rend = beerLiquid.GetComponent<Renderer>();//get the renderer from the gameobject
        fillLevel.y = 0.10f;//set fill level
        rend.material.SetVector("_fillLevel", fillLevel);//reference names in shader graph so that it matches the fill level in this script


        PCStartRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//default rotation

        PCEndRotation = Quaternion.Euler(-135.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//rotates 90degrees towards player

    }




    public void lowerFillLevel()
    {
        //lower fill level
          
                // decrease fill level over time
                fillLevel.y = Mathf.Lerp(fillLevel.y, -0.5f, fillElaspsedTime/lerpDuration);

            //send over to shader new value of fill level
            rend.material.SetFloat("_fillLevel", fillLevel.y);//reference names in shader graph
            Debug.Log("fillLevel: " + fillLevel.y);
            Debug.Log("fill down");

            fillElaspsedTime += Time.deltaTime;

            rend.material.SetVector("_fillLevel", fillLevel);//reference names in shader graph


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
        //if (cupNetObj.TryGetComponent<NetworkObject>(out cupNetObj))
        //{
            cupNetObj.GetComponent<NetworkObject>().ChangeOwnership(rpcParams.Receive.SenderClientId);

            rotateCupClientRpc();
        //}
    }


    [ClientRpc]
    private void rotateCupClientRpc()
    {
            rotateCup();
        
    }









    IEnumerator destroyCup(GameObject cupObj)
    {
        //play poof soundFX
        //show poof effect(particles?)
        //cupNetObj = cupNetObj.GetComponent<NetworkObject>();
        yield return new WaitForSeconds(3); //waits 3 seconds
        //cupNetObj.Despawn();
        if (beerLiquid.GetComponent<startBlurEffect>().Player1Drink == true)
        {//if player 1 needs to drink
            Volume playerVolume = gameManager.player1.GetComponent<Volume>();//get their volume
                                                                            
            playerVolume.profile.TryGet(out blurEffect);
            blurEffect.focalLength.value += 100;//increase the focal length value


            beerLiquid.GetComponent<startBlurEffect>().Player1Drink = false; // set back bool to false

        }
        else if (beerLiquid.GetComponent<startBlurEffect>().Player2Drink == true)
        {
            //gameManager.player2.GetComponent<Volume>().profile = ;//get their volume
            beerLiquid.GetComponent<startBlurEffect>().Player2Drink = false; // set back to false
        }
        //cupNetObj.SetActive(false);//hide the cup
        NetworkObject cupNetObj = cupObj.GetComponent<NetworkObject>();
        cupNetObj.DestroyWithScene = true;

    }


    //For VR
    //If gameobject.rotation.x < 90//being  poured/rotated
    //lowerFillLevel
    //destroy gameobject


}
