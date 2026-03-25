using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TicTacToeGameManagerNet : NetworkBehaviour
{
    [Header("Tiles")]
    public TicTacToeTileSlotNet[] tiles;

    [Header("Spawn")]
    public GameObject xPrefab;
    public GameObject oPrefab;
    public Transform pieceSpawnPoint;

    [Header("UI")]
    public GameObject startPanel;
    public GameObject restartButton;
    public TMP_Text resultText;

    [Header("Debug")]
    public bool debugLogs = true;

    private readonly NetworkVariable<int> currentTurn = new NetworkVariable<int>((int)TicTacToePieceType.X);
    private readonly NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);
    private readonly NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);

    private ulong xPlayerId;
    private ulong oPlayerId;
    private bool playerIdsReady = false;

    private void Start()
    {
        SetupLocalUI();
    }

    public override void OnNetworkSpawn()
    {
        SetupLocalUI();

        if (IsServer)
        {
            CachePlayerIds();
            currentTurn.Value = (int)TicTacToePieceType.X;
            gameOver.Value = false;
            gameStarted.Value = false;
        }
    }

    void SetupLocalUI()
    {
        if (startPanel != null)
            startPanel.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(false);

        if (resultText != null)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CachePlayerIds()
    {
        List<ulong> ids = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
        ids.Sort();

        if (ids.Count >= 1)
            xPlayerId = ids[0];

        if (ids.Count >= 2)
            oPlayerId = ids[1];
        else
            oPlayerId = ids[0];

        playerIdsReady = ids.Count >= 1;
    }

    public void StartGame()
    {
        if (IsServer)
            StartGameServer();
        else
            StartGameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void StartGameServerRpc()
    {
        StartGameServer();
    }

    void StartGameServer()
    {
        CachePlayerIds();

        gameStarted.Value = true;
        gameOver.Value = false;
        currentTurn.Value = (int)TicTacToePieceType.X;

        StartGameClientRpc();
        SpawnTurnPiece();
    }

    [ClientRpc]
    void StartGameClientRpc()
    {
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

        if (debugLogs)
            Debug.Log("Game Started");
    }

    public void RestartGame()
    {
        if (IsServer)
            RestartGameServer();
        else
            RestartGameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void RestartGameServerRpc()
    {
        RestartGameServer();
    }

    void RestartGameServer()
    {
        NetworkManager.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    IEnumerator SpawnNextPieceNextFrame()
    {
        yield return null;
        SpawnTurnPiece();
    }

    void SpawnTurnPiece()
    {
        if (!IsServer) return;
        if (gameOver.Value || !gameStarted.Value) return;

        if (!playerIdsReady)
            CachePlayerIds();

        TicTacToePieceType turnType = (TicTacToePieceType)currentTurn.Value;
        GameObject prefab = turnType == TicTacToePieceType.X ? xPrefab : oPrefab;

        if (prefab == null)
        {
            Debug.LogWarning("Spawn failed: prefab missing for " + turnType);
            return;
        }

        if (pieceSpawnPoint == null)
        {
            Debug.LogWarning("Spawn failed: pieceSpawnPoint missing.");
            return;
        }

        GameObject newPieceObj = Instantiate(prefab, pieceSpawnPoint.position, pieceSpawnPoint.rotation);
        NetworkObject netObj = newPieceObj.GetComponent<NetworkObject>();

        if (netObj == null)
        {
            Debug.LogWarning("Spawn failed: prefab has no NetworkObject.");
            Destroy(newPieceObj);
            return;
        }

        ulong ownerId = turnType == TicTacToePieceType.X ? xPlayerId : oPlayerId;
        netObj.SpawnWithOwnership(ownerId, true);

        PieceNet piece = newPieceObj.GetComponent<PieceNet>();
        if (piece != null)
            piece.MarkPlaced(false);

        Rigidbody rb = newPieceObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (debugLogs)
            Debug.Log("Spawned new piece: " + turnType + " for client " + ownerId);
    }

    public bool TryPlacePieceFromCollision(PieceNet held, TicTacToeTileSlotNet tile)
    {
        if (held == null || tile == null) return false;
        if (!held.IsOwner) return false;

        RequestPlacePieceServerRpc(
            held.GetComponent<NetworkObject>(),
            tile.GetComponent<NetworkObject>()
        );

        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    void RequestPlacePieceServerRpc(
        NetworkObjectReference pieceRef,
        NetworkObjectReference tileRef,
        ServerRpcParams rpcParams = default)
    {
        if (!gameStarted.Value)
        {
            if (debugLogs) Debug.Log("Place fail: game not started");
            return;
        }

        if (gameOver.Value)
        {
            if (debugLogs) Debug.Log("Place fail: game already over");
            return;
        }

        if (!pieceRef.TryGet(out NetworkObject pieceObj) || pieceObj == null)
        {
            if (debugLogs) Debug.Log("Place fail: invalid piece ref");
            return;
        }

        if (!tileRef.TryGet(out NetworkObject tileObj) || tileObj == null)
        {
            if (debugLogs) Debug.Log("Place fail: invalid tile ref");
            return;
        }

        PieceNet held = pieceObj.GetComponent<PieceNet>();
        TicTacToeTileSlotNet tile = tileObj.GetComponent<TicTacToeTileSlotNet>();

        if (held == null || tile == null)
        {
            if (debugLogs) Debug.Log("Place fail: piece or tile missing component");
            return;
        }

        ulong senderId = rpcParams.Receive.SenderClientId;
        TicTacToePieceType turnType = (TicTacToePieceType)currentTurn.Value;
        ulong expectedOwner = turnType == TicTacToePieceType.X ? xPlayerId : oPlayerId;

        if (senderId != expectedOwner)
        {
            if (debugLogs) Debug.Log("Place fail: wrong player turn");
            return;
        }

        if (held.OwnerClientId != senderId)
        {
            if (debugLogs) Debug.Log("Place fail: sender does not own piece");
            return;
        }

        if (held.IsPlaced)
        {
            if (debugLogs) Debug.Log("Place fail: piece already placed");
            return;
        }

        if (held.Type != turnType)
        {
            if (debugLogs) Debug.Log("Place fail: wrong piece type for turn");
            return;
        }

        if (!tile.CanPlace())
        {
            if (debugLogs) Debug.Log("Place fail: tile already filled");
            return;
        }

        tile.PlacePiece(held);

        if (CheckWinner(turnType))
        {
            gameOver.Value = true;
            ShowResultClientRpc(turnType + " Wins!");

            if (debugLogs) Debug.Log(turnType + " wins!");
            return;
        }

        if (IsBoardFull())
        {
            gameOver.Value = true;
            ShowResultClientRpc("Draw!");

            if (debugLogs) Debug.Log("Draw!");
            return;
        }

        currentTurn.Value = turnType == TicTacToePieceType.X
            ? (int)TicTacToePieceType.O
            : (int)TicTacToePieceType.X;

        if (debugLogs)
            Debug.Log("Placed successfully. Next turn: " + (TicTacToePieceType)currentTurn.Value);

        StartCoroutine(SpawnNextPieceNextFrame());
    }

    [ClientRpc]
    void ShowResultClientRpc(string message)
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

        foreach (TicTacToeTileSlotNet tile in tiles)
        {
            if (tile == null || !tile.IsOccupied())
                return false;
        }

        return true;
    }
}