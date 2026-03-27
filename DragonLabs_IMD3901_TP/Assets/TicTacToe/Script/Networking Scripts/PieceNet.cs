using UnityEngine;
using Unity.Netcode;

public class PieceNet : NetworkBehaviour
{
    [SerializeField] private TicTacToePieceType type = TicTacToePieceType.X;
    public TicTacToePieceType Type => type;

    private readonly NetworkVariable<bool> isPlaced = new NetworkVariable<bool>(false);
    public bool IsPlaced => isPlaced.Value;

    private Rigidbody rb;
    private bool placeRequestSent = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        isPlaced.OnValueChanged += OnPlacedChanged;
    }

    public override void OnNetworkDespawn()
    {
        isPlaced.OnValueChanged -= OnPlacedChanged;
    }

    private void OnPlacedChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            ForceReleaseFromPickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryAutoPlace(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryAutoPlace(other);
    }

    private void TryAutoPlace(Collider other)
    {
        if (!IsOwner) return;
        if (IsPlaced) return;
        if (placeRequestSent) return;

        TicTacToeTileSlotNet tile = other.GetComponent<TicTacToeTileSlotNet>();
        if (tile == null)
            tile = other.GetComponentInParent<TicTacToeTileSlotNet>();

        if (tile == null) return;
        if (!tile.CanPlace()) return;

        ForceReleaseFromPickup();

        placeRequestSent = true;
        tile.TryAutoPlacePiece(this);
    }

    private void ForceReleaseFromPickup()
    {
        PickupControllerNet pickup = FindFirstObjectByType<PickupControllerNet>();
        if (pickup != null)
        {
            pickup.ForceClearHeldObject();
        }

        transform.SetParent(null, true);

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    public void MarkPlaced(bool value)
    {
        if (IsServer)
            isPlaced.Value = value;
    }

    public void SnapToTile(Transform targetPoint)
    {
        if (targetPoint == null) return;

        transform.SetParent(null, true);
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        MarkPlaced(true);
    }
}