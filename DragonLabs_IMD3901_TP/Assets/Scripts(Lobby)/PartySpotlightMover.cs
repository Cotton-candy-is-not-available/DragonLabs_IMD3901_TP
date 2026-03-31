using UnityEngine;

public class PartySpotlightMover : MonoBehaviour
{
    [Header("Movement")]
    public float rotateSpeed = 25f;
    public float maxXAngle = 35f;
    public float maxYAngle = 50f;

    [Header("Color")]
    public Light targetLight;
    public bool cycleColors = true;
    public float colorChangeSpeed = 1.5f;

    [Header("Pulse")]
    public bool pulseIntensity = true;
    public float baseIntensity = 12f;
    public float pulseAmount = 3f;
    public float pulseSpeed = 2f;

    private float seed;
    private Quaternion startRotation;

    private void Start()
    {
        startRotation = transform.localRotation;
        seed = Random.Range(0f, 1000f);

        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }

        if (targetLight != null)
        {
            targetLight.intensity = baseIntensity;
        }
    }

    private void Update()
    {
        MoveLight();

        if (targetLight != null)
        {
            if (cycleColors)
            {
                CycleLightColor();
            }

            if (pulseIntensity)
            {
                PulseLightIntensity();
            }
        }
    }

    private void MoveLight()
    {
        float x = Mathf.Sin((Time.time + seed) * rotateSpeed * 0.02f) * maxXAngle;
        float y = Mathf.Cos((Time.time + seed) * rotateSpeed * 0.015f) * maxYAngle;

        transform.localRotation = startRotation * Quaternion.Euler(x, y, 0f);
    }

    private void CycleLightColor()
    {
        float hue = Mathf.Repeat((Time.time + seed) * colorChangeSpeed * 0.1f, 1f);
        Color newColor = Color.HSVToRGB(hue, 1f, 1f);
        targetLight.color = newColor;
    }

    private void PulseLightIntensity()
    {
        float pulse = Mathf.Sin((Time.time + seed) * pulseSpeed) * pulseAmount;
        targetLight.intensity = baseIntensity + pulse;
    }
}