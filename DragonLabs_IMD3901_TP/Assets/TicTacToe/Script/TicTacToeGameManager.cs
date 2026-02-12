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
        if (playerCamera == null)
        {
            if (debugLogs) Debug.Log("Place fail: playerCamera is NULL");
            return;
        }

        Piece held = GetHeldPieceFromHoldPoint();
        if (held == null)
        {
            if (debugLogs) Debug.Log("Place fail: not holding a Piece (nothing under HoldPoint)");
            return;
        }

        if (held.Type != currentTurn)
        {
            if (debugLogs) Debug.Log("Place fail: wrong turn. Holding " + held.Type + " but turn is " + currentTurn);
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            if (debugLogs) Debug.Log("Place fail: raycast hit NOTHING (aim at a tile)");
            return;
        }

        // IMPORTANT: your collider might be on the tile object itself or child
        TicTacToeTileSlot tile = hit.collider.GetComponentInParent<TicTacToeTileSlot>();
        if (tile == null)
        {
            if (debugLogs) Debug.Log("Place fail: raycast hit '" + hit.collider.name + "' but it has NO TicTacToeTileSlot in parents");
            return;
        }

        if (!tile.CanPlace())
        {
            if (debugLogs) Debug.Log("Place fail: tile already has a piece");
            return;
        }

        // release from pickup system so it stops following hold point
        if (pickupController != null)
            pickupController.Drop();

        tile.PlacePiece(held, faceCameraWhenPlaced, playerCamera.transform);

        currentTurn = (currentTurn == TicTacToePieceType.X) ? TicTacToePieceType.O : TicTacToePieceType.X;
        SpawnTurnPiece();

        if (debugLogs) Debug.Log("Placed successfully ✅ Next turn: " + currentTurn);
    }

    Piece GetHeldPieceFromHoldPoint()
    {
        if (holdPoint == null) return null;
        return holdPoint.GetComponentInChildren<Piece>();
    }
}
