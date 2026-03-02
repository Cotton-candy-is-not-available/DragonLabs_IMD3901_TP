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
}
