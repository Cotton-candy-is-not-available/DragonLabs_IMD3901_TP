using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public float range = 0.5f;
    private float speed = 0.5f;
    private Vector3 startPos;

    private bool isPressed = false;
    private bool isAnimating = false;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            Press();
        }

        Vector3 target = isPressed ? startPos - Vector3.up * range : startPos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, target) < 0.001f)
        {
            if (isPressed)
                isPressed = false;
            else
                isAnimating = false;
        }
    }

    public void Press()
    {
        isPressed = true;
        isAnimating = true;
        Debug.Log("Pressed");
    }
}
