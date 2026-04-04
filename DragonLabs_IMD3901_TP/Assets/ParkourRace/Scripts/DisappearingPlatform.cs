using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    //var
    public float disappearTime = 3f;
    public float visableTime = 6f;

    private Renderer[] renderers;
    private Collider[] colliders;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        StartCoroutine(PlatformCycle());
    }

    IEnumerator PlatformCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(visableTime);
            foreach (Renderer r in renderers)
                r.enabled = false;

            foreach (Collider c in colliders)
                c.enabled = false;

            yield return new WaitForSeconds(disappearTime);
            foreach (Renderer r in renderers)
                r.enabled = true;

            foreach (Collider c in colliders)
                c.enabled = true;
        }
    }
}
