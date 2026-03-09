using TreeEditor;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public CharacterController charController;
    public Transform camTransform;
    private float xRotation = 0.0f;

    public Camera PcCamera;

    public GameObject PCplayer;
    public GameObject VRplayer;


    [SerializeField] string currentScene;

    [Header("----- Lobby spawn points -----")]

    public Transform lobbyP1SpawnPoint;
    public Transform lobbyP2SpawnPoint;



    [Header("----- Beer pong spawn points -----")]

    public Transform beerPongP1SpawnPoint;
    public Transform beerPongP2SpawnPoint;


    [Header("----- PiñataPop spawn points -----")]

    public Transform piñataPopP1SpawnPoint;
    public Transform piñataPopSpawnPoint;

    [Header("----- Parkour race spawn points -----")]

    public Transform parkourRaceP1SpawnPoint;
    public Transform parkourRaceP2SpawnPoint;


    [Header("----- Tic tac toe spawn points -----")]

    public Transform tickTacToeP1SpawnPoint;
    public Transform tickTacToeP2SpawnPoint;

    public override void OnNetworkSpawn()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (!IsOwner)
        {
            PcCamera.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked; //locks the cursor to the screen, so it moves with the camera
        Cursor.visible = false;

        //spawn positions
        //Print player ids

        //if client is player 1
        Debug.Log("Client  id: "+ NetworkManager.Singleton.LocalClientId);
        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            //gameObject.transform.transform.position = beerPongP1SpawnPoint.position;

            switch (currentScene)
            {
                //case "Lobby":
                //    gameObject.transform.transform.position = lobbyP1SpawnPoint.position;
                //    break;

                case "beerPong":
                    Debug.Log("beer pong scene");
                    //gameObject.transform.transform.position = GameObject.FindGameObjectWithTag("p1SpawnPoint").GetComponent<Transform>().position;
                    //NetworkManager.Singleton.NetworkConfig.PlayerPrefab.transform.position = beerPongP1SpawnPoint.position;

                    break;

               //case "scene name here":
               //    gameObject.transform.transform.position = beerPongP2SpawnPoint.position;
               //    break;

            }
            Debug.Log("Player 1 spawned");
        }

        //if cliet is player 2
        else if (NetworkManager.Singleton.LocalClientId == 1)
        {
            switch (currentScene)
            {
                //case "Lobby":
                //    gameObject.transform.transform.position = lobbyP2SpawnPoint.position;
                //    break;

                case "beerPong":
                    gameObject.transform.transform.position = beerPongP2SpawnPoint.position;
                    break;

                //case "scene name here":
                //    gameObject.transform.transform.position = beerPongP2SpawnPoint.position;
                //    break;




            }
                Debug.Log("Player 2 spawned");

        }




    }



    //private NetworkObject SpawnPlayer(ulong clientId)
    //{
    //    var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
    //    player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, false);

    //    Debug.Log($"Player spawned with id {clientId}");

    //    return player.GetComponent<NetworkObject>();
    //}


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
    }
}
