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

    public void MarkPlaced(bool value) => isPlaced = value;
    public void SetHeld(bool value) => isHeld = value;

    public void ApplyHoldPose()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void SetPhysicsHeld(bool held)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
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
}