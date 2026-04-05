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
    public GameObject restartButton;
    public TMP_Text resultText;

    [Header("Debug")]
    public bool debugLogs = true;

    private TicTacToePieceType currentTurn = TicTacToePieceType.X;
    private bool gameOver = false;
    private bool gameStarted = false;

    public ChooseGame chooseGame_access;

    private void Start()
    {
        if (staticClass.PCOn)
        {
            // Get PC player references if the player chose PC mode
            playerCamera = GameObject.FindWithTag("localPlayerCamera").GetComponent<Camera>();
            holdPoint = playerCamera.transform.GetChild(0);
            pickupController = playerCamera.GetComponent<PickupController>();
        }

        gameOver = false;
        gameStarted = true;
        currentTurn = TicTacToePieceType.X;

        // Lock cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (restartButton != null)
            restartButton.SetActive(false);

        if (resultText != null)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }

        // Spawn the first piece automatically
        SpawnTurnPiece();

        if (debugLogs)
            Debug.Log("Game started automatically.");
    }

    private void Update()
    {
        if (!gameStarted || gameOver)
            return;

        bool placePressed =
            (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (placePressed)
            TryPlaceLookingAtTile();
    }

    public void RestartGame()
    {
        // Unlock cursor and reload current scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private Piece GetHeldPieceFromHoldPoint()
    {
        if (holdPoint == null)
            return null;

        return holdPoint.GetComponentInChildren<Piece>();
    }

    private IEnumerator SpawnNextPieceNextFrame()
    {
        // Wait one frame before spawning the next piece
        yield return null;
        SpawnTurnPiece();
    }

    private void SpawnTurnPiece()
    {
        if (gameOver || !gameStarted)
            return;

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
            // Set starting physics for spawned piece
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (debugLogs)
            Debug.Log("Spawned new piece: " + currentTurn + " at " + spawnPos);
    }

    private void TryPlaceLookingAtTile()
    {
        if (playerCamera == null)
        {
            if (debugLogs)
                Debug.Log("Place failed: playerCamera missing.");
            return;
        }

        Piece held = GetHeldPieceFromHoldPoint();

        if (held == null)
        {
            if (debugLogs)
                Debug.Log("Place failed: not holding a piece.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, placeDistance);

        TicTacToeTileSlot tile = null;

        foreach (RaycastHit hit in hits)
        {
            // Ignore pieces while raycasting
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
            if (debugLogs)
                Debug.Log("Place failed: not looking at a tile.");
            return;
        }

        TryPlacePieceFromCollision(held, tile);
    }

    public bool TryPlacePieceFromCollision(Piece held, TicTacToeTileSlot tile)
    {
        if (!gameStarted)
        {
            if (debugLogs)
                Debug.Log("Place failed: game not started.");
            return false;
        }

        if (gameOver)
        {
            if (debugLogs)
                Debug.Log("Place failed: game already over.");
            return false;
        }

        if (held == null)
        {
            if (debugLogs)
                Debug.Log("Place failed: no piece.");
            return false;
        }

        if (tile == null)
        {
            if (debugLogs)
                Debug.Log("Place failed: no tile.");
            return false;
        }

        if (held.IsPlaced)
        {
            if (debugLogs)
                Debug.Log("Place failed: piece already placed.");
            return false;
        }

        if (held.Type != currentTurn)
        {
            if (debugLogs)
                Debug.Log("Place failed: wrong turn.");
            return false;
        }

        if (!tile.CanPlace())
        {
            if (debugLogs)
                Debug.Log("Place failed: tile already filled.");
            return false;
        }

        if (pickupController != null)
            pickupController.Drop();

        // Place the piece on the tile
        tile.PlacePiece(held);

        if (CheckWinner(currentTurn))
        {
            gameOver = true;
            ShowResult(currentTurn + " Wins!");

            if (AudioManagerSinglePlayer.instance != null)
            {
                AudioManagerSinglePlayer.instance.PlaySFX(
                    AudioManagerSinglePlayer.instance.ttt_winSound
                );
            }

            if (debugLogs)
                Debug.Log(currentTurn + " wins!");

            return true;
        }

        if (IsBoardFull())
        {
            gameOver = true;
            ShowResult("Draw!");

            if (AudioManagerSinglePlayer.instance != null)
            {
                AudioManagerSinglePlayer.instance.PlaySFX(
                    AudioManagerSinglePlayer.instance.ttt_drawSound
                );
            }

            if (debugLogs)
                Debug.Log("Draw!");

            return true;
        }

        // Switch to the next player's turn
        currentTurn = currentTurn == TicTacToePieceType.X
            ? TicTacToePieceType.O
            : TicTacToePieceType.X;

        if (debugLogs)
            Debug.Log("Placed successfully. Next turn: " + currentTurn);

        StartCoroutine(SpawnNextPieceNextFrame());
        return true;
    }

    private void ShowResult(string message)
    {
        // Show result UI and unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (resultText != null)
        {
            resultText.text = message;
            resultText.gameObject.SetActive(true);
        }

        //if (restartButton != null)
        //    restartButton.SetActive(true);
        StartCoroutine(waitToSwitchScene());
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

    private bool TileMatches(int index, TicTacToePieceType pieceType)
    {
        if (tiles == null || index < 0 || index >= tiles.Length || tiles[index] == null)
            return false;

        TicTacToePieceType? tileType = tiles[index].GetOccupyingType();
        return tileType.HasValue && tileType.Value == pieceType;
    }

    private bool IsBoardFull()
    {
        if (tiles == null || tiles.Length == 0)
            return false;

        foreach (TicTacToeTileSlot tile in tiles)
        {
            if (tile == null || !tile.IsOccupied())
                return false;
        }
       
        return true;
    }
    IEnumerator waitToSwitchScene()
    {
        Debug.Log("called waitToSwitch");
        //yield on a new YieldInstruction that waits for 15 seconds.
        yield return new WaitForSeconds(15);

        //change scenes back to the lobby for the host and client 
        chooseGame_access.switchScenes("Lobby");
    }

}