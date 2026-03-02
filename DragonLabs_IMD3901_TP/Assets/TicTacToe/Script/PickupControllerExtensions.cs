using UnityEngine;
using System.Reflection;

public static class PickupControllerExtensions
{
    static Transform GetHoldTransform(PickupController pc)
    {
        if (pc == null) return null;

        var t = pc.GetType();

        // Try common field / property names (based on your inspector: "Hold Area")
        string[] names = { "holdArea", "HoldArea", "holdPoint", "HoldPoint", "holdTransform", "HoldTransform" };

        foreach (string n in names)
        {
            // field
            FieldInfo f = t.GetField(n, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null && f.FieldType == typeof(Transform))
                return (Transform)f.GetValue(pc);

            // property
            PropertyInfo p = t.GetProperty(n, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.PropertyType == typeof(Transform))
                return (Transform)p.GetValue(pc);
        }

        return null;
    }

    static Piece GetHeldPiece(Transform hold)
    {
        if (hold == null) return null;
        return hold.GetComponentInChildren<Piece>();
    }

    // Team script calls: pickupController_access.Grab(piece);
    public static void Grab(this PickupController pc, Piece piece)
    {
        if (pc == null || piece == null) return;

        Transform hold = GetHoldTransform(pc);
        if (hold == null) return;

        // If already holding something, drop it first
        Piece current = GetHeldPiece(hold);
        if (current != null && current != piece)
            pc.Drop();

        piece.transform.SetParent(hold, true);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.identity;

        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        piece.SetHeld(true);
    }

    // Team script calls: pickupController_access.Drop();
    public static void Drop(this PickupController pc)
    {
        if (pc == null) return;

        Transform hold = GetHoldTransform(pc);
        if (hold == null) return;

        Piece held = GetHeldPiece(hold);
        if (held == null) return;

        held.transform.SetParent(null, true);

        Rigidbody rb = held.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        held.SetHeld(false);
    }
}
