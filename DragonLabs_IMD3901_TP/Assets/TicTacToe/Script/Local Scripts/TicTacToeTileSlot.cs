using UnityEngine;

public class TicTacToeTileSlot : MonoBehaviour
{
    [Header("Per-piece spawn points")]
    public Transform xSpawnPoint;
    public Transform oSpawnPoint;

    private Piece placedPiece;
    private TicTacToePieceType? occupyingType = null;

    public bool CanPlace()
    {
        return placedPiece == null;
    }

    public bool IsOccupied()
    {
        return placedPiece != null;
    }

    public TicTacToePieceType? GetOccupyingType()
    {
        return occupyingType;
    }

    public void PlacePiece(Piece piece)
    {
        if (piece == null) return;
        if (placedPiece != null) return;

        Transform targetPoint = GetTargetPoint(piece.Type);
        if (targetPoint == null)
        {
            Debug.LogWarning("Missing correct spawn point on tile: " + gameObject.name);
            return;
        }

        placedPiece = piece;
        occupyingType = piece.Type;

        piece.SnapToTile(targetPoint);
        piece.MarkPlaced(true);
        piece.SetHeld(false);
    }

    public bool TryAutoPlacePiece(Piece piece)
    {
        if (piece == null) return false;
        if (placedPiece != null) return false;
        if (piece.IsPlaced) return false;

        TicTacToeGameManager gameManager = FindFirstObjectByType<TicTacToeGameManager>();
        if (gameManager == null)
        {
            Debug.LogWarning("No TicTacToeGameManager found in scene.");
            return false;
        }

        return gameManager.TryPlacePieceFromCollision(piece, this);
    }

    private Transform GetTargetPoint(TicTacToePieceType pieceType)
    {
        if (pieceType == TicTacToePieceType.X)
            return xSpawnPoint;

        return oSpawnPoint;
    }

    public void ClearTile()
    {
        placedPiece = null;
        occupyingType = null;
    }
}