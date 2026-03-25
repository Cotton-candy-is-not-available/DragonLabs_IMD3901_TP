using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PieceNet : NetworkBehaviour
{
    [SerializeField] private TicTacToePieceType type = TicTacToePieceType.X;
    public TicTacToePieceType Type => type;

    private readonly NetworkVariable<bool> isPlaced = new NetworkVariable<bool>(false);
    private readonly NetworkVariable<bool> isHeld = new NetworkVariable<bool>(false);

    public bool IsPlaced => isPlaced.Value;
    public bool IsHeld => isHeld.Value;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        if (IsPlaced) return;

        TicTacToeTileSlotNet tile = other.GetComponent<TicTacToeTileSlotNet>();
        if (tile == null)
            tile = other.GetComponentInParent<TicTacToeTileSlotNet>();

        if (tile == null) return;

        tile.TryAutoPlacePiece(this);
    }

    public void MarkPlaced(bool value)
    {
        if (IsServer)
            isPlaced.Value = value;
    }

    public void SetHeld(bool value)
    {
        if (IsServer)
            isHeld.Value = value;
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
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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

        if (IsServer)
        {
            isPlaced.Value = true;
            isHeld.Value = false;
        }

        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
        }

        transform.SetParent(null);
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;

        if (rb != null)
            rb.isKinematic = true;

        if (grabInteractable != null)
            grabInteractable.enabled = false;
    }
}