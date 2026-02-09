using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PickupController : MonoBehaviour
{
    [SerializeField] Transform holdArea; //will be parented to this

    //the object that is picked up
    private GameObject heldObj;
    public GameObject HeldObject => heldObj;
    private Rigidbody heldObjRB;

    //physics
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    // Get current scene name
    Scene currentScene;

    //----- For throwing trgectory: Beer Pong---
    //For objects that need to be thrown
    public float throwForce = 500f;
    [SerializeField] tragectoryLine line;
    public float mass = 10;
    bool enableLine = false;
    //------------------------------------------

    private void Start()
    {
        // Get current scene name
        currentScene = SceneManager.GetActiveScene();
    }




    private void Update()
    {
        enableLine = false;//turn off the line by default --Beer Pong


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

            //Tragectory Line--------------------
            if (currentScene.name == "beerPong")//only enable in beerPong scene
            {
                enableLine = true;
            }
            else
            {
                enableLine = false;
            }
            //-----------------------------------
        }

        //----Draw the tragectory line BeerPong
        line.drawTragectory(transform.forward * throwForce, enableLine);
        //-----------------------------------


    }

    /*----------------FUNCTIONS---------------*/
    void pickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            //make the object float
            heldObjRB.useGravity = false;
            heldObjRB.linearDamping = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation; //avoids spinning

            heldObjRB.transform.parent = holdArea; //parent it to the hold area
            heldObj = pickObj;
        }
    }
    void dropObject()
    {
        //make the object float
        heldObjRB.useGravity = true;
        heldObjRB.linearDamping = 1;
        heldObjRB.constraints = RigidbodyConstraints.None; //remove constraints
        heldObj.transform.parent = null; //unparent it from the hold area

        //if the current scene is the beer pong minigame, add force so that the object can be thrown
        if (currentScene.name == "beerPong")
        {
            //heldObjRB.AddForce(transform.forward * throwForce);
            Debug.Log("beerpong scene");
            heldObjRB.linearVelocity = transform.forward * throwForce;
        }

        heldObj = null; //no object being held anymore
    }
    void moveObject()
    {
        /*since the object is being parented to holdArea (which moves with the player's camera)
        we have to calculate the difference between it's current position and the new position that the holdArea
        is located at/looking at*/
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce); //moev the object in the calculated direction
        }
    }
}
