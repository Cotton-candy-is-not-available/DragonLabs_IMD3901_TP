using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor.PackageManager;
using UnityEngine;

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

    Quaternion startPos;
    Quaternion targetPos;
    bool isPressed = false;
    bool isAnimating = false;

    public NetworkObject cupNetObj;



    private void Start()
    {
        rend = beerLiquid.GetComponent<Renderer>();//get the renderer from the gameobject
    }


   



    //private void Update()
    //{
        //if (VRObject.activeSelf)//allow for pouring when VR mode is on
        //{
        //    VRPouring();

        //    //Debug.Log("transform.rotation.z " + gameObject.transform.localRotation.z);
        //    //Debug.Log("transform.rotation.y " + gameObject.transform.localRotation.y);
        //    //Debug.Log("transform.rotation.x " + gameObject.transform.localRotation.x);

        //}

    //}


    public void startPouringDebug()
    {

        //if (rotationProgress < 1 && rotationProgress >= 0)
        //{
        rotationProgress += Time.deltaTime * 5;//slowly rotate

        //transform.rotation = Quaternion.Lerp(PCStartRotation, PCEndRotation, rotationProgress);//rotates watering can smoothly
        gameObject.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
        Debug.Log("start rotating");
        //water.SetActive(true);//turn on water particles
        //}

    }

    void Update()
    {
        if (isAnimating)
        {
            Debug.Log("UPdate isAnimating" + isAnimating);
            NetworkObject.transform.localRotation = Quaternion.Euler(0, 0, 90f);
        }
            Debug.Log("UPdate isAnimating" + isAnimating);

    }
    public void startPouring()
    {
        isPressed = true;
        isAnimating = true;
    }

    [ServerRpc(RequireOwnership = false)] //client requests to server to press the tile
    public void startPouringServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (cupNetObj.TryGetComponent<NetworkObject>(out cupNetObj))
        {
            //transfer ownership so the client can interact with it (playerId here is SenderClientId)
            cupNetObj.ChangeOwnership(rpcParams.Receive.SenderClientId);

            //animate the tile now that the client has ownership over it
            startPouringClientRpc();
        }
    }

    [ClientRpc] //run the tile animation on the client
    private void startPouringClientRpc()
    {
        if (IsOwner)
        {
            startPouring();
            Debug.Log("client isAnimating"+ isAnimating);

            //Debug.Log("CLIENT RPC ANIMATE CALLED");
        }
    }






    public void VRPouring()
    {
        Vector3 wateringCanRotation = cupNetObj.transform.eulerAngles;
        Debug.Log("euler rotation" + wateringCanRotation.z);

        if (wateringCanRotation.z >= VRPourRotation || wateringCanRotation.x >= VRPourRotation)// if rotated more than VRPourRotation then start pouring
        {
            //Debug.Log("POUR");
            //Debug.Log("rot.z more " + wateringCanRotation.z);


            //water.SetActive(true);//turn on water particles

        }
        else if (wateringCanRotation.z <= VRPourRotation || wateringCanRotation.x <= VRPourRotation)//if not rotating at VRPourRotation turn off water
        {
            //Debug.Log("NOT POUR");
            //Debug.Log("rot.z less " + wateringCanRotation.z);

            //water.SetActive(false);//turn OFF water particles

        }

    }

    public void StopPouring()
    {

        //transform.localRotation = Quaternion.identity;//set begining rotation to parent

        //PCStartRotation = transform.rotation;//default rotation

        //PCEndRotation = Quaternion.Euler(90.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        //rotationProgress = 0;

        //if (transform.rotation == PCStartRotation)
        //{
            Debug.Log("stop pouring");
            //water.SetActive(false);//turn off water particles

        //}


    }




    private void lowerFillLevel()
    {
        //lower fill level
        time += Time.deltaTime;
        // decrease fill level over time
        fillLevel = Mathf.Lerp(fillLevel, 0, Time.deltaTime);

        //send over to shader new value of fill level
        rend.material.SetFloat("_fillLevel", fillLevel);//reference names in shader graph
        Debug.Log("fill down");

    }


}
