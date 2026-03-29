using UnityEngine;
using System.Reflection;

public static class PickupControllerExtensionsNet
{
    static Transform GetHoldTransform(PickupController pc)
    {
        if (pc == null) return null;

        var t = pc.GetType();

        string[] names =
        {
            "holdArea", "HoldArea",
            "holdPoint", "HoldPoint",
            "holdTransform", "HoldTransform"
        };

        foreach (string n in names)
        {
            FieldInfo f = t.GetField(n, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f.FieldType == typeof(Transform))
                return (Transform)f.GetValue(pc);

            PropertyInfo p = t.GetProperty(n, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(Transform))
                return (Transform)p.GetValue(pc);
        }

        return null;
    }

    static PieceNet GetHeldPiece(Transform hold)
    {
        if (hold == null) return null;
        return hold.GetComponentInChildren<PieceNet>();
    }

    public static void GrabNet(this PickupController pc, PieceNet piece)
    {
        if (pc == null || piece == null) return;

        Transform hold = GetHoldTransform(pc);
        if (hold == null) return;

        PieceNet current = GetHeldPiece(hold);
        if (current != null && current != piece)
            pc.DropNet();

        piece.transform.SetParent(hold, false);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.identity;

        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    public static void DropNet(this PickupController pc)
    {
        if (pc == null) return;

        Transform hold = GetHoldTransform(pc);
        if (hold == null) return;

        PieceNet held = GetHeldPiece(hold);
        if (held == null) return;

        held.transform.SetParent(null, true);

        Rigidbody rb = held.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (!held.IsPlaced)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            else
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

    public static void ForceClearHeldPieceNet(this PickupController pc)
    {
        if (pc == null) return;

        Transform hold = GetHoldTransform(pc);
        if (hold == null) return;

        PieceNet held = GetHeldPiece(hold);
        if (held == null) return;

        held.transform.SetParent(null, true);

        Rigidbody rb = held.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    public static PieceNet GetHeldPieceNet(this PickupController pc)
    {
        Transform hold = GetHoldTransform(pc);
        if (hold == null) return null;

        return hold.GetComponentInChildren<PieceNet>();
    }
}