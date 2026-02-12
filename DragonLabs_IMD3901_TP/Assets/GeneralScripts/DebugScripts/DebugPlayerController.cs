using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugPlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public CharacterController charController;
    public Transform camTransform;
    private float xRotation = 0.0f;

    public Camera PcCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 moveInput = Keyboard.current != null ? new Vector2
            (
                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
            ) : Vector2.zero;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        charController.Move(move * speed * Time.deltaTime);

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 80f);

        camTransform.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
