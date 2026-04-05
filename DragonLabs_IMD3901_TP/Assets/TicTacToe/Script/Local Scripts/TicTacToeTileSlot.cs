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
        if (piece == null)
            return;

        if (placedPiece != null)
            return;

        // Get the correct snap point depending on X or O
        Transform targetPoint = GetTargetPoint(piece.Type);

        if (targetPoint == null)
        {
            Debug.LogWarning("Missing correct spawn point on tile: " + gameObject.name);
            return;
        }

        // Store what piece is now on this tile
        placedPiece = piece;
        occupyingType = piece.Type;

        // Snap the piece into place
        piece.SnapToTile(targetPoint);
        piece.MarkPlaced(true);
        piece.SetHeld(false);
    }

    public bool TryAutoPlacePiece(Piece piece)
    {
        if (piece == null)
            return false;

        if (placedPiece != null)
            return false;

        if (piece.IsPlaced)
            return false;

        // Find the game manager and let it handle the placement rules
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
        // Reset tile data if the board gets cleared
        placedPiece = null;
        occupyingType = null;
    }
}