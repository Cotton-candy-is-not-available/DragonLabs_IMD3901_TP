using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class startGame : NetworkBehaviour
{
    [Header("---Bools----")]

    //bools will be called in other scenes to determine which prefabs and function need to be in use
    public bool VRMode = false;//need to make don't destroy onload
    public bool PCMode = false;//need to make don't destroy onload

    //need to make don't destroy onload
    public bool multiPlayerMode = false;
    public bool singlePlayerMode = false;

    [Header("---Panels & canvases----")]

    //Panels to hide and show
    public GameObject gameSteupCanvas;
    public GameObject startPanel;
    public GameObject plateformOptionPanel;
    public GameObject gameModeOptionPanel;
    public GameObject startNetPanel;

    [Header("---Player prefabs ----")]
    //Network player prefabs
    public GameObject PCplayer;
    public GameObject VRplayer;

    //local single players
    public GameObject localPCPlayer;
    public GameObject localVRPlayer;

    [Header("--- Network manager ----")]
    //networkmanager to activate
    public GameObject NetworkManagerObject;

    //[Header("--- Join strings ----")]
    //public GameObject IPAdressText;
    //public GameObject joinCodeText;

    [Header("--- Start camera ----")]
    //public GameObject mainCamera;

    public ChooseGame chooseGameAccess;

   
    public NetworkVariable<bool> clientStarted = new NetworkVariable<bool>();//make static



    public override void OnNetworkSpawn()
    {
        clientStarted.Value = false;
        //clientStarted.OnValueChanged += OnP1HitPointsChanged;

    }

    private void Start()
    {
        clientStarted.Value = false;//client has not started

        //if (hasStarted.gameHasStarted)//if the game has already started turn off camera and tdon't run the function
        //{
        //    //mainCamera.enabled = false;//turn off the main camera
        //    //mainCamera.SetActive(false);//turn off the main camera


        //    return;//don't run start panel if game has already started
        //}
         //mainCamera.SetActive(true);//turn on the main camera
        //mainCamera.enabled = true;//turn on the main camera
        //turn on start game panel by default
        startPanel.SetActive(true);//show start panel 

        localPCPlayer.SetActive(false);//turn local PC off by default
        localVRPlayer.SetActive(false);//turn local VR off by default

    }

    //start button function to get players into the game
    public void startButton()
    {
        startPanel.SetActive(false);//hides start panel on click
        plateformOptionPanel.SetActive(true);//shows choose plateform panel on click

    }


    //if user chooses to use VR
    public void VROptionButton()
    {
        plateformOptionPanel.SetActive(false);//hides choose plateform panel on click
        gameModeOptionPanel.SetActive(true);//shows choose game mode panel on click
        VRMode = true;//player chose to use VR headset to play

    }

    //If user chooses to use PC
    public void PCOptionButton()
    {
        plateformOptionPanel.SetActive(false);//hides choose plateform panel on click
        gameModeOptionPanel.SetActive(true);//shows choose game mode panel on click
        PCMode = true;//player chose to use computer to play
    }


    //If user chooses the multiplayer option
    public void multiPlayerOptionButton()
    {
        gameModeOptionPanel.SetActive(false);//hides choose game mode panel on click
        startNetPanel.SetActive(true);//show network option(host, client, server)

        multiPlayerMode = true;//player chose to play with unity networking


        if (PCMode)//if pc button was clicked earlier
        {
            NetworkManagerObject.SetActive(true);//activate the networkmanager

            NetworkManager.Singleton.NetworkConfig.PlayerPrefab = PCplayer;
            //PCNetworkManager.SetActive(true);//activate PC network manager

        }
        else if (VRMode)//if VR button was clicked earlier
        {
            NetworkManagerObject.SetActive(true);//activate the networkmanager

            NetworkManager.Singleton.NetworkConfig.PlayerPrefab = VRplayer;

            //VRNetworkManager.SetActive(true);//activate VR network manager

        }
    }

    //If user chooses the single player option
    public void singlePlayerOptionButton()
    {
        gameModeOptionPanel.SetActive(false);//hides choose game mode panel on click

        gameSteupCanvas.SetActive(false);//hides canvas

        singlePlayerMode = true;//player chose to play by themselves without networking

        staticClass.SingleON = singlePlayerMode;//tell lobby that its single player and to hide join codes

        if (PCMode)//if pc button was clicked earlier
        {
            localPCPlayer.SetActive(true);//activate local PC 
            //mainCamera.enabled = false;//turn off the main camera
            //mainCamera.SetActive(false);//turn off the main camera
            chooseGameAccess.switchScenes("Lobby");//Move player to lobby


        }
        else if (VRMode)//if VR button was clicked earlier
        {
            localVRPlayer.SetActive(true);//activate local VR 
            //mainCamera.enabled = false;//turn off the main camera
            //mainCamera.SetActive(false);//turn off the main camera
            chooseGameAccess.switchScenes("Lobby");//Move player to lobby



        }
        //disactivate ability to go to beer pong
    }





    //--------------LAN Network buttons--------------------

    public void startHost()
    {
        //IPAdressText.SetActive(true);//shows ip address to connect to
        NetworkManager.Singleton.StartHost();//start host
        gameSteupCanvas.SetActive(false );//hide net connect panel
        hasStarted.gameHasStarted = true;//set to true so that wehn the player comes bakc in the scene this fruntion does not run again
        Debug.Log("Host started LAN");

        //clientStarted.Value = true;//client has started
        //clientStartServerRpc();
        //mainCamera.enabled = false;//turn off the main camera
        //mainCamera.SetActive(false);//turn off the main camera
        chooseGameAccess.switchScenesNetServerRpc("Lobby");

        staticClass.LANOn = true;


    }

    public void startClient()
    {

        //IPAdressText.SetActive(false);//hides ip address if not already
        NetworkManager.Singleton.StartClient();//join game as client
        gameSteupCanvas.SetActive(false);//hide net connect panel
        hasStarted.gameHasStarted = true;//set to true so that wehn the player comes bakc in the scene this fruntion does not run again
        clientStartServerRpc();//client has started
        Debug.Log("Client started LAN");
        //mainCamera.enabled = false;//turn off the main camera
        //mainCamera.SetActive(false);//turn off the main camera
        chooseGameAccess.switchScenesNetServerRpc("Lobby");



    }

    [ServerRpc(RequireOwnership = false)] //host and client are able to ask the server to update the teampoints
    public void clientStartServerRpc()
    {
        //increasae the value of points on both host and client since its a network variable
        clientStarted.Value = true;
        Debug.Log("clientStarted RPC: LAN");
    }


}
