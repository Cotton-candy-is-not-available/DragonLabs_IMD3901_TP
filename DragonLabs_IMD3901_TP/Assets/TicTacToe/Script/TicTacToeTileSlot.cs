using UnityEngine;

public class TicTacToeTileSlot : MonoBehaviour
{
    [Header("Spawn Point on this tile")]
    public Transform spawnPoint;

    private Piece placedPiece;

    public bool CanPlace()
    {
        return placedPiece == null;
    }

    public void PlacePiece(Piece piece, bool faceCamera, Transform cameraTransform)
    {
        if (piece == null) return;
        if (!CanPlace()) return;
        if (spawnPoint == null) return;

        placedPiece = piece;

        // snap to tile
        piece.transform.SetParent(spawnPoint, true);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.identity;

        // optional: face camera
        if (faceCamera && cameraTransform != null)
        {
            Vector3 dir = cameraTransform.position - piece.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
                piece.transform.rotation = Quaternion.LookRotation(-dir.normalized, Vector3.up);
        }

        // stick: turn off physics
        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        piece.SetHeld(false);
        piece.MarkPlaced(true);
    }
}
