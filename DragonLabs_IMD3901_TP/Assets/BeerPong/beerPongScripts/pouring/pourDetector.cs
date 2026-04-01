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

    public GameObject cupObj;


    float fillElaspsedTime;
    float lerpDuration = 3;

    public gameManager gameManager;
    public DepthOfField blurEffect;


    public NetworkVariable<bool> turnOffCup = new NetworkVariable<bool>(false);


    private void Start()
    {
        rend = beerLiquid.GetComponent<Renderer>();//get the renderer from the gameobject
        fillLevel.y = -0.23f;//set fill level
        rend.material.SetVector("_fillLevel", fillLevel);//reference names in shader graph so that it matches the fill level in this script


        PCStartRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//default rotation

        PCEndRotation = Quaternion.Euler(-135.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//rotates 90degrees towards player

    }


    private void Update()
    {
        Debug.Log("cupObj.transform.rotation.x: " + cupObj.transform.rotation.x);

        if (staticClass.VROn)
        { //if vr is enabled
            Debug.Log("VR ON pour");

            if (cupObj.transform.rotation.x <= -0.5 || cupObj.transform.rotation.x >= 0.5)// if rotated then start pouring
            {
                Debug.Log("POUR VRRRRR");
                //Debug.Log("rot.z more " + wateringCanRotation.z);

                lowerFillLevelServerRPC();//lower the fill level

            }
        }

    }


    public void lowerFillLevel()
    {
        //lower fill level
            rend.material.SetVector("_fillLevel", fillLevel);//reference names in shader graph

        // decrease fill level over time
            //fillLevel.y = Mathf.Lerp(fillLevel.y, -0.5f, fillElaspsedTime/lerpDuration);

            //send over to shader new value of fill level
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

            StartCoroutine(destroyCup(cupObj));//destoy the cup

    }



    [ServerRpc(RequireOwnership = false)]
    public void rotateCupServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        //if (cupNetObj.TryGetComponent<NetworkObject>(out cupNetObj))
        //{
            cupObj.GetComponent<NetworkObject>().ChangeOwnership(rpcParams.Receive.SenderClientId);

            rotateCupClientRpc();
        //}
    }


    [ClientRpc]
    private void rotateCupClientRpc()
    {
            rotateCup();
        
    }

    [ServerRpc(RequireOwnership = false)]
    void lowerFillLevelServerRPC()
    {
        lowerFillLevel();
        StartCoroutine(destroyCup(cupObj));//destoy the cup

    }



    IEnumerator destroyCup(GameObject cupObj)
    {
        NetworkObject cupNetObj = cupObj.GetComponent<NetworkObject>();
        cupNetObj.DestroyWithScene = true;
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
        cupObj.SetActive(turnOffCup.Value);//hide the cup
        

    }


    //For VR
    //If gameobject.rotation.x < 90//being  poured/rotated
    //lowerFillLevel
    //destroy gameobject


}
