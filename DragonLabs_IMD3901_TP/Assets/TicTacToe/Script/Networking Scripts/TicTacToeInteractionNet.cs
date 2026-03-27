using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class TicTacToeInteractionNet : NetworkBehaviour
{
    public float placeRange = 5f;
    public Camera playerCamera;
    public PickupController pickupController;

    private TicTacToeGameManagerNet gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<TicTacToeGameManagerNet>();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (playerCamera == null || pickupController == null) return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryPlaceHeldPiece();
        }
    }

    void TryPlaceHeldPiece()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<TicTacToeGameManagerNet>();

        if (gameManager == null)
        {
            Debug.LogWarning("No GameManager found");
            return;
        }

        PieceNet heldPiece = pickupController.GetHeldPieceNet();
        if (heldPiece == null)
        {
            Debug.Log("No held piece");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, placeRange))
        {
            TicTacToeTileSlotNet tile = hit.collider.GetComponent<TicTacToeTileSlotNet>();

            if (tile == null)
                tile = hit.collider.GetComponentInParent<TicTacToeTileSlotNet>();

            if (tile == null)
            {
                Debug.Log("Not looking at tile");
                return;
            }

            pickupController.DropNet();
            gameManager.TryPlacePieceFromCollision(heldPiece, tile);
        }
    }
}