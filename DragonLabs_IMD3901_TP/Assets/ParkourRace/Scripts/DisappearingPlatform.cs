using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearTime = 3f;
    public float visableTime = 6f;

    private Renderer platformRenderer;
    private Collider platformCollider;

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();

        StartCoroutine(PlatformCycle());
    }

    IEnumerator PlatformCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(visableTime);
            platformRenderer.enabled = false;
            platformCollider.enabled = false;

            yield return new WaitForSeconds(disappearTime);
            platformRenderer.enabled = true;
            platformCollider.enabled = true;
        }
    }
}
