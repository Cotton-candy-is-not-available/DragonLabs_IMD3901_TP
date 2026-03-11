using UnityEngine;
using UnityEngine.InputSystem;

public class TicTacToeGameManager : MonoBehaviour
{
    [Header("Tiles")]
    public TicTacToeTileSlot[] tiles;

    [Header("Spawn")]
    public GameObject xPrefab;
    public GameObject oPrefab;
    public Transform pieceSpawnPoint;

    [Header("Player Refs")]
    public Camera playerCamera;
    public Transform holdPoint;
    public PickupController pickupController;

    [Header("Raycast")]
    public float placeDistance = 10f;

    [Header("Debug")]
    public bool debugLogs = true;

    private TicTacToePieceType currentTurn = TicTacToePieceType.X;

    void Start()
    {
        SpawnTurnPiece();
    }

    void Update()
    {
        bool placePressed =
            (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (placePressed)
            TryPlaceLookingAtTile();
    }

    Piece GetHeldPieceFromHoldPoint()
    {
        if (holdPoint == null) return null;
        return holdPoint.GetComponentInChildren<Piece>();
    }

    void SpawnTurnPiece()
    {
        GameObject prefab = currentTurn == TicTacToePieceType.X ? xPrefab : oPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Spawn failed: prefab is missing.");
            return;
        }

        if (pieceSpawnPoint == null)
        {
            Debug.LogWarning("Spawn failed: pieceSpawnPoint is missing.");
            return;
        }

        GameObject newPiece = Instantiate(prefab, pieceSpawnPoint.position, pieceSpawnPoint.rotation);

        Piece piece = newPiece.GetComponent<Piece>();
        if (piece != null)
        {
            piece.MarkPlaced(false);
            piece.SetHeld(false);
        }

        Rigidbody rb = newPiece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (debugLogs) Debug.Log("Spawned new piece: " + currentTurn);
    }

    void TryPlaceLookingAtTile()
    {
        if (playerCamera == null)
        {
            if (debugLogs) Debug.Log("Place fail: playerCamera missing");
            return;
        }

        Piece held = GetHeldPieceFromHoldPoint();
        if (held == null)
        {
            if (debugLogs) Debug.Log("Place fail: not holding a piece");
            return;
        }

        if (held.Type != currentTurn)
        {
            if (debugLogs) Debug.Log("Place fail: wrong turn");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, placeDistance);

        TicTacToeTileSlot tile = null;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponentInParent<Piece>() != null)
                continue;

            tile = hit.collider.GetComponent<TicTacToeTileSlot>();
            if (tile == null)
                tile = hit.collider.GetComponentInParent<TicTacToeTileSlot>();

            if (tile != null)
                break;
        }

        if (tile == null)
        {
            if (debugLogs) Debug.Log("Place fail: not looking at a tile");
            return;
        }

        if (!tile.CanPlace())
        {
            if (debugLogs) Debug.Log("Place fail: tile already filled");
            return;
        }

        if (pickupController != null)
            pickupController.Drop();

        tile.PlacePiece(held);

        currentTurn = currentTurn == TicTacToePieceType.X ? TicTacToePieceType.O : TicTacToePieceType.X;

        if (debugLogs) Debug.Log("Placed successfully. Next turn: " + currentTurn);

        SpawnTurnPiece();
    }
}