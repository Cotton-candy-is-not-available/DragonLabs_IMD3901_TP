using UnityEngine;
using Unity.Netcode;

public class PieceNet : NetworkBehaviour
{
    [SerializeField] private TicTacToePieceType type = TicTacToePieceType.X;
    public TicTacToePieceType Type => type;

    private readonly NetworkVariable<bool> isPlaced = new NetworkVariable<bool>(false);
    public bool IsPlaced => isPlaced.Value;

    private Rigidbody rb;
    private Collider[] allColliders;
    private bool placeRequestSent = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        allColliders = GetComponentsInChildren<Collider>(true);
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
            ReleaseFromPickupAndFreeze();
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

        // important: fully let go before asking server to place
        ReleaseFromPickupAndFreeze();

        placeRequestSent = true;
        tile.TryAutoPlacePiece(this);
    }

    private void ReleaseFromPickupAndFreeze()
    {
        PickupController pickup = FindFirstObjectByType<PickupController>();
        if (pickup != null)
        {
            pickup.DropNet();
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

        // detach from anything still holding it
        transform.SetParent(targetPoint, false);

        // force exact local alignment to the spawn point
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // optional: stop more trigger spam after placement
        if (allColliders != null)
        {
            foreach (Collider c in allColliders)
            {
                if (c != null)
                    c.enabled = false;
            }
        }

        MarkPlaced(true);
    }
}