using UnityEngine;

public class TicTacToeTileSlot : MonoBehaviour
{
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

        // SNAP to tile spawn point (this breaks it away from HoldPoint)
        piece.transform.SetParent(spawnPoint, true);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.identity;

        // face camera (optional)
        if (faceCamera && cameraTransform != null)
        {
            Vector3 dir = cameraTransform.position - piece.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
                piece.transform.rotation = Quaternion.LookRotation(-dir.normalized, Vector3.up);
        }

        // LOCK physics
        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // IMPORTANT: stop pickup from re-grabbing it
        // easiest: remove Interactable tag after placing
        piece.gameObject.tag = "Untagged";

        piece.SetHeld(false);
        piece.MarkPlaced(true);
    }
}
