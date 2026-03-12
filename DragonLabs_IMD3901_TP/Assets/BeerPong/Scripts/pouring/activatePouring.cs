using System;
using System.Collections;
using Unity.Netcode;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class activatePouring : NetworkBehaviour
{
    //wil be added to player when in  beer pong scene
    Scene currentScene;

    pourDetector pouring;

    public PickupControllerNet PickupControllerNet;

    public GameObject held;

    public Transform holdArea;

    [SerializeField] float rotationProgress;
    [SerializeField] Quaternion PCStartRotation;
    [SerializeField] Quaternion PCEndRotation;

    private void Start()
    {
        // Get current scene name
        currentScene = SceneManager.GetActiveScene();

        PCStartRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//default rotation

        PCEndRotation = Quaternion.Euler(-90.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);//rotates 90degrees towards player


    }
    // Update is called once per frame
    void Update()
    {
        //held = PickupControllerNet.heldObj;

        //if (currentScene.name == "beerPong"){//only enable in beerPong scene

        //if (PickupControllerNet.heldObj != null && PickupControllerNet.heldObj.layer == 7 || PickupControllerNet.heldObj != null && PickupControllerNet.heldObj.layer == 8)//if the player is holding an object and that object is on the P1Cup of P2Cup layers
            if (PickupControllerNet.heldObj != null )
        {
            Debug.Log("not null");

                if (Keyboard.current.pKey.isPressed) //if p is pressed to drink
                {
                    Debug.Log("p key was pressed net pouring");
                //pouring = PickupControllerNet.heldObj.GetComponent<pourDetector>();//get the cups pour detector script
                //holdArea.rotation = Quaternion.Euler(90f, 0f, 0f);
                rotationProgress += Time.deltaTime * 5;//somewhat slowly rotate; Note: smaller number slower, bigger number faster
                holdArea.rotation = Quaternion.Lerp(PCStartRotation, PCEndRotation, rotationProgress);//rotates watering can smoothly
                pouring.lowerFillLevel();//lower the liquid inside the cup

                StartCoroutine(destroyCup(PickupControllerNet.heldObj));//destoy the cup

                //Destroy(PickupControllerNet.heldObj);
                //pouring.startPouringServerRpc();
                //pouring.startPouringServerRpc(PickupControllerNet.heldObj.GetComponent<NetworkObject>().NetworkObjectId);
            }
            else
                {
                    pouring = PickupControllerNet.heldObj.GetComponent<pourDetector>();//get the cups pour detector script
                    //pouring.StopPouring();//stop pouring
                    Debug.Log("else STOP");

                }
        }
        //}
    }




    IEnumerator destroyCup(GameObject heldObj)
    {
        //play poof soundFX
        //show poof effect(particles?)
        yield return new WaitForSeconds(3); //waits 3 seconds
        Destroy(heldObj); //destroy the cup
    }







}
