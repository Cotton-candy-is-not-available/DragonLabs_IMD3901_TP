using UnityEngine;

public enum TicTacToePieceType { X, O }

public class Piece : MonoBehaviour
{
    [SerializeField] private TicTacToePieceType type = TicTacToePieceType.X;
    public TicTacToePieceType Type => type;

    private bool isPlaced = false;
    private bool isHeld = false;

    public bool IsPlaced => isPlaced;
    public bool IsHeld => isHeld;

    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        TicTacToeTileSlot tile = other.GetComponent<TicTacToeTileSlot>();
        if (tile == null)
            tile = other.GetComponentInParent<TicTacToeTileSlot>();

        if (tile == null) return;

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
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void SetPhysicsHeld(bool held)
    {
        if (rb == null) return;

        if (held)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void SnapToTile(Transform targetPoint)
    {
        if (targetPoint == null) return;

        isPlaced = true;
        isHeld = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
        }

        transform.SetParent(null);
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
        transform.SetParent(targetPoint);

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }
    }
}