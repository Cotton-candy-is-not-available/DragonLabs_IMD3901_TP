using UnityEngine;
using UnityEngine.InputSystem;

public class playerInteraction : MonoBehaviour
{
    public float interactRange = 5f;
    public Camera playerCamera;

    public Crosshair crosshair_access;
    public PickupController pickupController_access;

    public delegate void InteractionEvent(GameObject interactableObj);
    public static event InteractionEvent onInteract;
   
    void Update()
    {
        //creating a ray that shoots out of the camera to detect objects in front of us
        //we want the ray to be in front of us
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit; //hit box for the ray cast

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                //checking if the ray hits something with a collider that is interactable
                crosshair_access.setInteract(true);

                if (Keyboard.current.iKey.wasPressedThisFrame) //if e was pressed to eat
                {
                    Debug.Log("i key was pressed to interact");
                }

                //Debug.Log("interact was set to true");
                return;
            }
        }
        crosshair_access.setInteract(false); //set it back to false if we look away from the object
    }
}
