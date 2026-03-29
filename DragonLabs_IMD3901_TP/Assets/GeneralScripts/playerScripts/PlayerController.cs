using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController Instance;

    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public CharacterController charController;
    public Transform camTransform;
    private float xRotation = 0.0f;

    public Camera PcCamera;

    public GameObject PCplayer;
    public GameObject VRplayer;
    bool isLocked = true;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            PcCamera.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked; //locks the cursor to the screen, so it moves with the camera
        Cursor.visible = false;

        
        //if client is player 1
        Debug.Log("Client  id: "+ NetworkManager.Singleton.LocalClientId);


        //check that there is only one object in the scene with this script

           //if there is another object with this script set this as player 2
        if (Instance != null)
        {
            Instance = this;
            gameObject.tag = "Player2";//give them the player 1 tag

            PcCamera.tag = "p2Camera";//set camera tags
            Debug.Log("P2 Camera tag: " + PcCamera.tag);

         
        }
        else//otherwise they are player 1
        {
            Instance = this;
            gameObject.tag = "Player1";//give them the player 1 tag

            PcCamera.tag = "p1Camera";//set camera tags
            Debug.Log("P1 Camera tag: " + PcCamera.tag);


        }

        Debug.Log("owner client ID" + (int)OwnerClientId);


        Debug.Log("isHost: " + IsHost + " IsClient: " + IsClient + " IsServer: " + IsServer);



    }






    void Update()
    {
        //Networking
        if (!IsOwner)
        {
            return;
        }

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



        //unlock and lock cursor when escape key is pressed
        if (Keyboard.current.uKey.wasPressedThisFrame)//press u to unlock cursor; change to escape
        {
            if (isLocked)
            {
                isLocked = !isLocked;
                Cursor.lockState = CursorLockMode.None; //locks the cursor to the screen, so it moves with the camera
                Cursor.visible = true;//shows cursor 
            }
            else
            {
                isLocked = !isLocked;
                Cursor.lockState = CursorLockMode.Locked; //unlocks the cursor to the screen
                Cursor.visible = false;//hides cursor 
            }
        }



    }
}
