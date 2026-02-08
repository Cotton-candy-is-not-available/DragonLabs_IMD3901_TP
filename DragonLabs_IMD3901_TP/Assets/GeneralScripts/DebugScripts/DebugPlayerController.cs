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
        Cursor.lockState = CursorLockMode.Locked; //locks the cursor to the screen, so it moves with the camera
        Cursor.visible = false;
    }


    void Update()
    {
        //-1 in the negative direction along x or y, +1 in the positive direction
        Vector2 moveInput = Keyboard.current != null ? new Vector2
            (
                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
            ) : Vector2.zero;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        charController.Move(move * speed * Time.deltaTime); //apply the movement to the player

        Vector2 mouseDelta = Mouse.current.delta.ReadValue(); //read the values from the mouse
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        //when we move our mouse up or down, we want the player to look up, not for the camera to flip
        //create a restriction and clamp the value
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 80f);

        //euler inputs a number in degrees
        camTransform.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX); //apply it to the camera
    }
}
