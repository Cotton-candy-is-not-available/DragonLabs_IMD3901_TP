using UnityEngine;

public class TicTacToeTileSlot : MonoBehaviour
{
    public Transform spawnPoint;

    private Piece placedPiece;

    public bool CanPlace()
    {
        return placedPiece == null;
    }

    public void PlacePiece(Piece piece)
    {
        if (piece == null) return;
        if (!CanPlace()) return;
        if (spawnPoint == null) return;

        placedPiece = piece;

        piece.transform.SetParent(spawnPoint, false);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.identity;

        piece.SetPhysicsHeld(true);

        piece.gameObject.tag = "Untagged";
        piece.SetHeld(false);
        piece.MarkPlaced(true);
    }
}