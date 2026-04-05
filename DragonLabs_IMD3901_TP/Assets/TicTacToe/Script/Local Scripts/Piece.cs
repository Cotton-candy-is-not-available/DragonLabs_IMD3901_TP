using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public enum TicTacToePieceType
{
    X,
    O
}

public class Piece : MonoBehaviour
{
    [SerializeField] private TicTacToePieceType type = TicTacToePieceType.X;
    public TicTacToePieceType Type => type;

    private bool isPlaced = false;
    private bool isHeld = false;

    public bool IsPlaced => isPlaced;
    public bool IsHeld => isHeld;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        // Get needed components when the object starts
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Do nothing if this piece was already placed
        if (isPlaced)
            return;

        // Try to find the tile slot from what we touched
        TicTacToeTileSlot tile = other.GetComponent<TicTacToeTileSlot>();

        if (tile == null)
            tile = other.GetComponentInParent<TicTacToeTileSlot>();

        if (tile == null)
            return;

        // Ask the tile to try placing this piece
        tile.TryAutoPlacePiece(this);
    }

    public void MarkPlaced(bool value)
    {
        isPlaced = value;
    }

    public void SetHeld(bool value)
    {
        isHeld = value;
    }

    public void ApplyHoldPose()
    {
        // Reset local position and rotation while being held
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void SetPhysicsHeld(bool held)
    {
        if (rb == null)
            return;

        if (held)
        {
            // While holding, stop physics from affecting it
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            // When released, turn physics back on
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void SnapToTile(Transform targetPoint)
    {
        if (targetPoint == null)
            return;

        isPlaced = true;
        isHeld = false;

        if (rb != null)
        {
            // Stop movement before snapping into place
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
        }

        // Move the piece to the correct tile spawn point
        transform.SetParent(null);
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;

        if (rb != null)
        {
            // Lock it after placing so it stays in place
            rb.isKinematic = true;
        }

        if (grabInteractable != null)
        {
            // Disable grabbing after the piece is placed
            grabInteractable.enabled = false;
        }
    }
}