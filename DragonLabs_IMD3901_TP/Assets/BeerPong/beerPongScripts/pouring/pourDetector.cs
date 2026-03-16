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

    public NetworkObject cupNetObj;



    private void Start()
    {
        rend = beerLiquid.GetComponent<Renderer>();//get the renderer from the gameobject
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
        Debug.Log("fill down");

    }

    //For VR
    //If gameobject.rotation.x < 90//being  poured/rotated
    //lowerFillLevel
    //destroy gameobject


}
