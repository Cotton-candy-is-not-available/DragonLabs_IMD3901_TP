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
        if (!CanPlace()) return;

        Transform targetSpawnPoint = GetSpawnPointForPiece(piece);

        if (targetSpawnPoint == null)
        {
            Debug.LogWarning($"No spawn point assigned for piece type {piece.Type} on tile {gameObject.name}");
            return;
        }

        placedPiece = piece;
        occupyingType = piece.Type;

        piece.transform.SetParent(targetSpawnPoint, false);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.identity;

        piece.SetPhysicsHeld(true);
        piece.gameObject.tag = "Untagged";
        piece.SetHeld(false);
        piece.MarkPlaced(true);
    }

    private Transform GetSpawnPointForPiece(Piece piece)
    {
        if (piece.Type == TicTacToePieceType.X)
            return xSpawnPoint;

        if (piece.Type == TicTacToePieceType.O)
            return oSpawnPoint;

        return null;
    }
}