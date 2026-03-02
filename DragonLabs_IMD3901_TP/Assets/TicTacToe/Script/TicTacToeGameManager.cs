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
    public Camera playerCamera;               // camera_PC
    public Transform holdPoint;               // camera_PC/HoldPoint
    public PickupController pickupController; // camera_PC PickupController

    [Header("Options")]
    public bool faceCameraWhenPlaced = true;

    [Header("Debug")]
    public bool debugLogs = true;

    private TicTacToePieceType currentTurn = TicTacToePieceType.X;
    Piece GetHeldPieceFromHoldPoint()
    {
        if (holdPoint == null) return null;
        return holdPoint.GetComponentInChildren<Piece>();
    }

    void Start()
    {
        SpawnTurnPiece();
    }

    void Update()
    {
        // NEW INPUT SYSTEM (works with your team scripts)
        bool placePressed =
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame); // E as backup

        if (placePressed)
            TryPlaceLookingAtTile();
    }

    void SpawnTurnPiece()
    {
        GameObject prefab = (currentTurn == TicTacToePieceType.X) ? xPrefab : oPrefab;
        if (prefab == null) return;

        if (pieceSpawnPoint != null)
        {
            Instantiate(prefab, pieceSpawnPoint.position, pieceSpawnPoint.rotation);
            return;
        }

        if (playerCamera != null)
        {
            Vector3 spawnPos = playerCamera.transform.position + playerCamera.transform.forward * 2f + Vector3.up * 0.2f;
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    void TryPlaceLookingAtTile()
    {
        if (playerCamera == null) return;

        Piece held = GetHeldPieceFromHoldPoint();
        if (held == null) { if (debugLogs) Debug.Log("Place fail: not holding a Piece"); return; }
        if (held.Type != currentTurn) { if (debugLogs) Debug.Log("Place fail: wrong turn"); return; }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, 10f);

        TicTacToeTileSlot tile = null;

        foreach (RaycastHit h in hits)
        {
            // ignore any piece (including the one you're holding)
            if (h.collider.GetComponentInParent<Piece>() != null)
                continue;

            tile = h.collider.GetComponentInParent<TicTacToeTileSlot>();
            if (tile != null) break;
        }

        if (tile == null) { if (debugLogs) Debug.Log("Place fail: no tile found"); return; }
        if (!tile.CanPlace()) { if (debugLogs) Debug.Log("Place fail: tile already filled"); return; }

        if (pickupController != null)
            pickupController.Drop();

        tile.PlacePiece(held, faceCameraWhenPlaced, playerCamera.transform);

        currentTurn = (currentTurn == TicTacToePieceType.X) ? TicTacToePieceType.O : TicTacToePieceType.X;
        SpawnTurnPiece();

        if (debugLogs) Debug.Log("Placed successfully ✅ Next turn: " + currentTurn);
    }

}
