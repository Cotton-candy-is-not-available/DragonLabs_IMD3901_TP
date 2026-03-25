using UnityEngine;
using System.Reflection;

public static class PickupControllerExtensions
{
    static Transform GetHoldTransform(PickupController pc)
    {
        if (pc == null) return null;

        var t = pc.GetType();

        string[] names = { "holdArea", "HoldArea", "holdPoint", "HoldPoint", "holdTransform", "HoldTransform" };

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

    static Piece GetHeldPiece(Transform hold)
    {
        if (hold == null) return null;
        return hold.GetComponentInChildren<Piece>();
    }

    public static void Grab(this PickupController pc, Piece piece)
    {
        if (pc == null || piece == null) return;

        Transform hold = GetHoldTransform(pc);
        if (hold == null) return;

        Piece current = GetHeldPiece(hold);
        if (current != null && current != piece)
            pc.Drop();

        piece.transform.SetParent(hold, false);
        piece.ApplyHoldPose();
        piece.SetPhysicsHeld(true);
        piece.SetHeld(true);
    }

    public static void Drop(this PickupController pc)
    {
        if (pc == null) return;

        Transform hold = GetHoldTransform(pc);
        if (hold == null) return;

        Piece held = GetHeldPiece(hold);
        if (held == null) return;

        held.transform.SetParent(null, true);
        held.SetPhysicsHeld(false);
        held.SetHeld(false);
    }
}