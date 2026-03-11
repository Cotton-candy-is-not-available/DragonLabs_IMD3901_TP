using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public float jumpHeight = 1.2f;
    public float gravity = -20f;
    public float groundCheckDistance = 0.2f;

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}
