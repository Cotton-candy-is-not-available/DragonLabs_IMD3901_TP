using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 5f;
    public Camera playerCamera;

    public Crosshair crosshair_access;
    public PickupController pickupController_access;

    void Update()
    {
        // DROP (Tab)
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            pickupController_access.Drop();
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                crosshair_access.setInteractServerRpc(true);

                // GRAB (I)
                if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
                {
                    Debug.Log("i key was pressed to interact");

                    // IMPORTANT: works even if collider is on a child
                    Piece piece = hit.collider.GetComponentInParent<Piece>();
                    if (piece != null)
                    {
                        pickupController_access.Grab(piece);
                    }
                }
                return;
            }
        }

        crosshair_access.setInteractServerRpc(false);
    }
}
