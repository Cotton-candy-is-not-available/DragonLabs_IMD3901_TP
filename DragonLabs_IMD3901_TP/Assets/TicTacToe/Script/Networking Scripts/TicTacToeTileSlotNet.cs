using UnityEngine;
using Unity.Netcode;

public class TicTacToeTileSlotNet : NetworkBehaviour
{
    public Transform xSpawnPoint;
    public Transform oSpawnPoint;

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
            Debug.LogWarning("Missing spawn point on tile: " + gameObject.name);
            return;
        }

        occupied.Value = true;
        occupyingType.Value = (int)piece.Type;

        piece.SnapToTile(targetPoint);

        Debug.Log("Placed " + piece.Type + " on " + gameObject.name);
    }

    public bool TryAutoPlacePiece(PieceNet piece)
    {
        if (piece == null) return false;
        if (occupied.Value) return false;
        if (piece.IsPlaced) return false;

        TicTacToeGameManagerNet gameManager = FindFirstObjectByType<TicTacToeGameManagerNet>();
        if (gameManager == null)
        {
            Debug.LogWarning("No TicTacToeGameManagerNet found.");
            return false;
        }

        return gameManager.TryPlacePieceFromCollision(piece, this);
    }

    private Transform GetTargetPoint(TicTacToePieceType pieceType)
    {
        return pieceType == TicTacToePieceType.X ? xSpawnPoint : oSpawnPoint;
    }
}