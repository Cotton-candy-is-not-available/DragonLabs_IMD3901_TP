using System.Globalization;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class scenePushButton : NetworkBehaviour
{
    public float interactRange = 2f;
    //Get both palyer cameras after calling join button in start script or start relay script
    public GameObject player1Cam;
    public GameObject player2Cam;

    public GameObject localPlayerCam;

    public float range = 1f;
    [SerializeField] float speed = 2f;
    private Vector3 startPos;

    private bool isPressed = false;
    private bool isAnimating = false;

    // public startGame LANJoinBool
    // public startGame singlePlayerBool
    public startGame startGameAccess;

    // public startRelay relayJoinBool


    [SerializeField] ChooseGame switchScenes;
    [SerializeField] string sceneName;


    NetworkObject netObjButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition;
        netObjButton = gameObject.GetComponent<NetworkObject>();//get network object tied to this game object
    }

    // Update is called once per frame
    void Update()
    {
        //If single player selected

        if (localPlayerCam != null)
        {
            Ray localRay = new Ray(localPlayerCam.GetComponent<Camera>().transform.position, localPlayerCam.GetComponent<Camera>().transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(localRay, out hit, interactRange))
            {
                if (hit.collider.CompareTag("button"))
                {
                    Debug.Log("button seen"); 

                    if (Keyboard.current.pKey.wasPressedThisFrame)
                    {
                        Debug.Log("key pressed");

                        localPress();//calls aniamtion bools
                    }
                }
            }


            //if collsion with VR hand


            //Animated button up and down
            Vector3 target = isPressed ? startPos - Vector3.up * range : startPos;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, target) < 0.001f)
            {
                if (isPressed)
                {
                    switchScenes.switchScenes(sceneName);

                    isPressed = false;
                }
                else
                    isAnimating = false;
            }

        }
        if (localPlayerCam == null && startGameAccess.localPCPlayer)
        {
            localPlayerCam = GameObject.FindGameObjectWithTag("localPlayerCamera");
        }



        //----------------- Networking -----------------
        if (player1Cam!=null && player2Cam !=null)//as long as player cameras exists
        //if (player1Cam!=null)//as long as player cameras exists
        {
            Debug.Log("got camera");

            Ray p1Ray = new Ray(player1Cam.GetComponent<Camera>().transform.position, player1Cam.GetComponent<Camera>().transform.forward);
            //Ray p2Ray = new Ray(player2Cam.transform.position, player2Cam.transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(p1Ray, out hit, interactRange))
            {
                if (hit.collider.CompareTag("button"))
                {
                    if (Keyboard.current.pKey.wasPressedThisFrame)
                    {
                        //Press();
                        PressTileServerRpc(netObjButton.NetworkObjectId);
                    }
                }
            }

            Vector3 target = isPressed ? startPos - Vector3.up * range : startPos;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, target) < 0.001f)
            {
                if (isPressed)
                {
                    switchScenes.switchScenesNet(sceneName);

                    isPressed = false;
                }
                else
                    isAnimating = false;
            }
        }

        Debug.Log("startGameAccess.clientStarted: " + startGameAccess.clientStarted.Value);

        if (player1Cam == null && player2Cam == null &&  startGameAccess.clientStarted.Value == true)//
        {
            Debug.Log("getting camera");
            player1Cam = GameObject.FindGameObjectWithTag("p1Camera");
            player2Cam = GameObject.FindGameObjectWithTag("p2Camera");
        }
       
    }

    public void localPress()
    {
        isPressed = true;
        isAnimating = true;
        Debug.Log("Local Pressed");
    }
    public void Press()
    {
        isPressed = true;
        isAnimating = true;
        Debug.Log("Pressed");
    }



    [ServerRpc(RequireOwnership = false)] //client requests to server to press the tile
    public void PressTileServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (netObjButton.TryGetComponent<NetworkObject>(out netObjButton))
        {
            //transfer ownership so the client can interact with it (playerId here is SenderClientId)
            netObjButton.ChangeOwnership(rpcParams.Receive.SenderClientId);

            //animate the tile now that the client has ownership over it
            AnimateTileClientRpc();
        }
    }

    [ClientRpc] //run the tile animation on the client
    private void AnimateTileClientRpc()
    {
        if (IsOwner)
        {
            Press();
            //Debug.Log("CLIENT RPC ANIMATE CALLED");
        }
    }




}
