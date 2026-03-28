using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TicTacToeGameManagerNet : NetworkBehaviour
{
    public TicTacToeTileSlotNet[] tiles;

    public GameObject xPrefab;
    public GameObject oPrefab;
    public Transform pieceSpawnPoint;

    public TMP_Text resultText;
    public bool debugLogs = true;

    private readonly NetworkVariable<int> currentTurn = new NetworkVariable<int>((int)TicTacToePieceType.X);
    private readonly NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);

    private ulong xPlayerId;
    private ulong oPlayerId;

    public override void OnNetworkSpawn()
    {
        if (resultText != null)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }

        if (IsServer)
        {
            StartCoroutine(SetupGame());
        }
    }

    private IEnumerator SetupGame()
    {
        yield return null;

        CachePlayerIds();

        gameOver.Value = false;
        currentTurn.Value = (int)TicTacToePieceType.X;

        SpawnTurnPiece();
    }

    private void CachePlayerIds()
    {
        List<ulong> ids = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
        ids.Sort();

        if (ids.Count == 0)
        {
            xPlayerId = 0;
            oPlayerId = 0;
        }
        else if (ids.Count == 1)
        {
            xPlayerId = ids[0];
            oPlayerId = ids[0];
        }
        else
        {
            xPlayerId = ids[0];
            oPlayerId = ids[1];
        }

        if (debugLogs)
        {
            Debug.Log("X Player ID: " + xPlayerId);
            Debug.Log("O Player ID: " + oPlayerId);
        }
    }

    private void SpawnTurnPiece()
    {
        if (!IsServer) return;
        if (gameOver.Value) return;

        GameObject prefab = currentTurn.Value == (int)TicTacToePieceType.X ? xPrefab : oPrefab;

        if (prefab == null || pieceSpawnPoint == null)
        {
            Debug.LogWarning("Spawn failed. Missing prefab or pieceSpawnPoint.");
            return;
        }

        GameObject obj = Instantiate(prefab, pieceSpawnPoint.position, pieceSpawnPoint.rotation);
        NetworkObject netObj = obj.GetComponent<NetworkObject>();

        if (netObj == null)
        {
            Debug.LogWarning("Piece prefab missing NetworkObject.");
            Destroy(obj);
            return;
        }

        ulong ownerId = currentTurn.Value == (int)TicTacToePieceType.X ? xPlayerId : oPlayerId;
        netObj.SpawnWithOwnership(ownerId, true);

        if (debugLogs)
            Debug.Log("Spawned " + ((TicTacToePieceType)currentTurn.Value) + " for client " + ownerId);
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
    private void RequestPlacePieceServerRpc(
        NetworkObjectReference pieceRef,
        NetworkObjectReference tileRef,
        ServerRpcParams rpcParams = default)
    {
        if (gameOver.Value) return;

        if (!pieceRef.TryGet(out NetworkObject pieceObj)) return;
        if (!tileRef.TryGet(out NetworkObject tileObj)) return;

        PieceNet held = pieceObj.GetComponent<PieceNet>();
        TicTacToeTileSlotNet tile = tileObj.GetComponent<TicTacToeTileSlotNet>();

        if (held == null || tile == null) return;

        ulong senderId = rpcParams.Receive.SenderClientId;
        TicTacToePieceType turnType = (TicTacToePieceType)currentTurn.Value;
        ulong expectedOwner = turnType == TicTacToePieceType.X ? xPlayerId : oPlayerId;

        if (senderId != expectedOwner) return;
        if (held.OwnerClientId != senderId) return;
        if (held.IsPlaced) return;
        if (held.Type != turnType) return;
        if (!tile.CanPlace()) return;

        tile.PlacePiece(held);

        if (CheckWinner(turnType))
        {
            gameOver.Value = true;
            ShowResultClientRpc(turnType + " Wins!");
            return;
        }

        if (IsBoardFull())
        {
            gameOver.Value = true;
            ShowResultClientRpc("Draw!");
            return;
        }

        currentTurn.Value = turnType == TicTacToePieceType.X
            ? (int)TicTacToePieceType.O
            : (int)TicTacToePieceType.X;

        StartCoroutine(SpawnNextPieceNextFrame());
    }

    private IEnumerator SpawnNextPieceNextFrame()
    {
        yield return null;
        SpawnTurnPiece();
    }

    [ClientRpc]
    private void ShowResultClientRpc(string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
            resultText.gameObject.SetActive(true);
        }

        Debug.Log(message);
    }

    private bool CheckWinner(TicTacToePieceType pieceType)
    {
        int[,] winPatterns = new int[,]
        {
            {0,1,2},
            {3,4,5},
            {6,7,8},
            {0,3,6},
            {1,4,7},
            {2,5,8},
            {0,4,8},
            {2,4,6}
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
        foreach (TicTacToeTileSlotNet tile in tiles)
        {
            if (tile == null || !tile.IsOccupied())
                return false;
        }

        return true;
    }
}