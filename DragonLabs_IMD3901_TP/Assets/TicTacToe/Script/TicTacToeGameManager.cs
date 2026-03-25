using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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

    [Header("UI")]
    public GameObject startPanel;
    public GameObject restartButton;
    public TMP_Text resultText;

    [Header("Debug")]
    public bool debugLogs = true;

    private TicTacToePieceType currentTurn = TicTacToePieceType.X;
    private bool gameOver = false;
    private bool gameStarted = false;

    void Start()
    {
        gameOver = false;
        gameStarted = false;
        currentTurn = TicTacToePieceType.X;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (startPanel != null)
            startPanel.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(false);

        if (resultText != null)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameStarted) return;
        if (gameOver) return;

        bool placePressed =
            (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (placePressed)
            TryPlaceLookingAtTile();
    }

    public void StartGame()
    {
        gameStarted = true;
        gameOver = false;
        currentTurn = TicTacToePieceType.X;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (startPanel != null)
            startPanel.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);

        if (resultText != null)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }

        SpawnTurnPiece();

        if (debugLogs) Debug.Log("Game Started");
    }

    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    Piece GetHeldPieceFromHoldPoint()
    {
        if (holdPoint == null) return null;
        return holdPoint.GetComponentInChildren<Piece>();
    }

    IEnumerator SpawnNextPieceNextFrame()
    {
        yield return null;
        SpawnTurnPiece();
    }

    void SpawnTurnPiece()
    {
        if (gameOver || !gameStarted) return;

        GameObject prefab = currentTurn == TicTacToePieceType.X ? xPrefab : oPrefab;

        if (prefab == null)
        {
            Debug.LogWarning("Spawn failed: prefab is missing for turn " + currentTurn);
            return;
        }

        if (pieceSpawnPoint == null)
        {
            Debug.LogWarning("Spawn failed: pieceSpawnPoint is missing.");
            return;
        }

        Vector3 spawnPos = pieceSpawnPoint.position;
        Quaternion spawnRot = pieceSpawnPoint.rotation;

        GameObject newPiece = Instantiate(prefab, spawnPos, spawnRot);
        newPiece.name = currentTurn + "_Piece_Spawned";

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

        if (debugLogs)
        {
            Debug.Log("Spawned new piece: " + currentTurn + " at " + spawnPos);
        }
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

        TryPlacePieceFromCollision(held, tile);
    }

    public bool TryPlacePieceFromCollision(Piece held, TicTacToeTileSlot tile)
    {
        if (!gameStarted)
        {
            if (debugLogs) Debug.Log("Place fail: game not started");
            return false;
        }

        if (gameOver)
        {
            if (debugLogs) Debug.Log("Place fail: game already over");
            return false;
        }

        if (held == null)
        {
            if (debugLogs) Debug.Log("Place fail: no piece");
            return false;
        }

        if (tile == null)
        {
            if (debugLogs) Debug.Log("Place fail: no tile");
            return false;
        }

        if (held.IsPlaced)
        {
            if (debugLogs) Debug.Log("Place fail: piece already placed");
            return false;
        }

        if (held.Type != currentTurn)
        {
            if (debugLogs) Debug.Log("Place fail: wrong turn");
            return false;
        }

        if (!tile.CanPlace())
        {
            if (debugLogs) Debug.Log("Place fail: tile already filled");
            return false;
        }

        if (pickupController != null)
            pickupController.Drop();

        tile.PlacePiece(held);

        if (CheckWinner(currentTurn))
        {
            gameOver = true;
            ShowResult(currentTurn + " Wins!");
            if (debugLogs) Debug.Log(currentTurn + " wins!");
            return true;
        }

        if (IsBoardFull())
        {
            gameOver = true;
            ShowResult("Draw!");
            if (debugLogs) Debug.Log("Draw!");
            return true;
        }

        currentTurn = currentTurn == TicTacToePieceType.X ? TicTacToePieceType.O : TicTacToePieceType.X;

        if (debugLogs) Debug.Log("Placed successfully. Next turn: " + currentTurn);

        StartCoroutine(SpawnNextPieceNextFrame());
        return true;
    }

    void ShowResult(string message)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (resultText != null)
        {
            resultText.text = message;
            resultText.gameObject.SetActive(true);
        }

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public bool CheckWinner(TicTacToePieceType pieceType)
    {
        int[,] winPatterns = new int[,]
        {
            { 0, 1, 2 },
            { 3, 4, 5 },
            { 6, 7, 8 },

            { 0, 3, 6 },
            { 1, 4, 7 },
            { 2, 5, 8 },

            { 0, 4, 8 },
            { 2, 4, 6 }
        };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (TileMatches(a, pieceType) &&
                TileMatches(b, pieceType) &&
                TileMatches(c, pieceType))
            {
                return true;
            }
        }

        return false;
    }

    bool TileMatches(int index, TicTacToePieceType pieceType)
    {
        if (tiles == null || index < 0 || index >= tiles.Length || tiles[index] == null)
            return false;

        TicTacToePieceType? tileType = tiles[index].GetOccupyingType();
        return tileType.HasValue && tileType.Value == pieceType;
    }

    bool IsBoardFull()
    {
        if (tiles == null || tiles.Length == 0) return false;

        foreach (TicTacToeTileSlot tile in tiles)
        {
            if (tile == null || !tile.IsOccupied())
                return false;
        }

        return true;
    }
}