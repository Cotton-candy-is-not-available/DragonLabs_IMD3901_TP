using UnityEngine;
using Unity.Netcode;

public class TicTacToeTileSlotNet : NetworkBehaviour
{
    [Header("Per-piece spawn points")]
    public Transform xSpawnPoint;
    public Transform oSpawnPoint;

    private PieceNet placedPiece;

    private readonly NetworkVariable<bool> occupied = new NetworkVariable<bool>(false);
    private readonly NetworkVariable<int> occupyingType = new NetworkVariable<int>(-1);

    public bool CanPlace()
    {
        return !occupied.Value;
    }

    public bool IsOccupied()
    {
        return occupied.Value;
    }

    public TicTacToePieceType? GetOccupyingType()
    {
        if (occupyingType.Value < 0)
            return null;

        return (TicTacToePieceType)occupyingType.Value;
    }

    public void PlacePiece(PieceNet piece)
    {
        if (!IsServer) return;
        if (piece == null) return;
        if (occupied.Value) return;

        Transform targetPoint = GetTargetPoint(piece.Type);
        if (targetPoint == null)
        {
            Debug.LogWarning("Missing correct spawn point on tile: " + gameObject.name);
            return;
        }

        placedPiece = piece;
        occupied.Value = true;
        occupyingType.Value = (int)piece.Type;

        piece.SnapToTile(targetPoint);
        piece.MarkPlaced(true);
    }

    public bool TryAutoPlacePiece(PieceNet piece)
    {
        if (piece == null) return false;
        if (occupied.Value) return false;
        if (piece.IsPlaced) return false;

        TicTacToeGameManagerNet gameManager = FindFirstObjectByType<TicTacToeGameManagerNet>();
        if (gameManager == null)
        {
            Debug.LogWarning("No TicTacToeGameManagerNet found in scene.");
            return false;
        }

        return gameManager.TryPlacePieceFromCollision(piece, this);
    }

    private Transform GetTargetPoint(TicTacToePieceType pieceType)
    {
        return pieceType == TicTacToePieceType.X ? xSpawnPoint : oSpawnPoint;
    }

    public void ClearTile()
    {
        if (!IsServer) return;

        placedPiece = null;
        occupied.Value = false;
        occupyingType.Value = -1;
    }
}