using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class startGame : NetworkBehaviour
{
    public AudioManagerSinglePlayer audioManager;

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
    [Header("---Net ----")]

    //Network player prefabs
    public GameObject PCplayer;
    public GameObject[] PCNetListplayer;
    public GameObject VRplayer;
    public GameObject[] VRNetListplayer;
    [Header("---Local ----")]
    //local single players
    public GameObject localPCPlayer;
    public GameObject localVRPlayer;

    [Header("--- Network manager ----")]
    //networkmanager to activate
    public GameObject NetworkManagerObject;


    [Header("--- Start camera ----")]
    //public GameObject mainCamera;

    public ChooseGame chooseGameAccess;


    public Button LANHostButton;
    public Button LANClientButton;


   
    private void Start()
    {

        startPanel.SetActive(true);//show start panel 

        localPCPlayer.SetActive(false);//turn local PC off by default
        localVRPlayer.SetActive(false);//turn local VR off by default

    }

    //start button function to get players into the game
    public void startButton()
    {
        audioManager.PlaySFX(audioManager.gmaeSetup_buttonClick);//play button click SFX when pressed
        startPanel.SetActive(false);//hides start panel on click
        plateformOptionPanel.SetActive(true);//shows choose plateform panel on click

    }


    //if user chooses to use VR
    public void VROptionButton()
    {
        audioManager.PlaySFX(audioManager.gmaeSetup_buttonClick);//play button click SFX when pressed

        plateformOptionPanel.SetActive(false);//hides choose plateform panel on click
        gameModeOptionPanel.SetActive(true);//shows choose game mode panel on click
        VRMode = true;//player chose to use VR headset to play
        staticClass.VROn = VRMode;//save to a static variable


    }

    //If user chooses to use PC
    public void PCOptionButton()
    {
        audioManager.PlaySFX(audioManager.gmaeSetup_buttonClick);//play button click SFX when pressed

        plateformOptionPanel.SetActive(false);//hides choose plateform panel on click
        gameModeOptionPanel.SetActive(true);//shows choose game mode panel on click
        PCMode = true;//player chose to use computer to play
        staticClass.PCOn = PCMode;//save to a static variable
    }


    //If user chooses the multiplayer option
    public void multiPlayerOptionButton()
    {
        audioManager.PlaySFX(audioManager.gmaeSetup_buttonClick);//play button click SFX when pressed

        gameModeOptionPanel.SetActive(false);//hides choose game mode panel on click
        startNetPanel.SetActive(true);//show network option(host, client, server)

        multiPlayerMode = true;//player chose to play with unity networking


        if (PCMode)//if pc button was clicked earlier
        {
            int randomNum = Random.Range(0, 4);//choose a random player from 0 to 3 in the array

            Debug.Log("random Num PC net: " + randomNum);//print the random num

            NetworkManagerObject.SetActive(true);//activate the networkmanager

            //NetworkManager.Singleton.NetworkConfig.PlayerPrefab = PCplayer;
            NetworkManager.Singleton.NetworkConfig.PlayerPrefab = PCNetListplayer[randomNum];//choose a random VR player from the list

        }
        else if (VRMode)//if VR button was clicked earlier
        {
            int randomNum = Random.Range(0, 4);//choose a random player from 0 to 3 in the array

            Debug.Log("random Num VR net: " + randomNum);//print the random num

            NetworkManagerObject.SetActive(true);//activate the networkmanager

            //NetworkManager.Singleton.NetworkConfig.PlayerPrefab = VRplayer;
            NetworkManager.Singleton.NetworkConfig.PlayerPrefab = VRNetListplayer[randomNum];//choose a random VR player from the list


        }
    }

    //If user chooses the single player option
    public void singlePlayerOptionButton()
    {
        audioManager.PlaySFX(audioManager.gmaeSetup_buttonClick);//play button click SFX when pressed

        gameModeOptionPanel.SetActive(false);//hides choose game mode panel on click

        gameSteupCanvas.SetActive(false);//hides canvas

        singlePlayerMode = true;//player chose to play by themselves without networking

        staticClass.SingleON = singlePlayerMode;//tell lobby that its single player and to hide join codes

        if (PCMode)//if pc button was clicked earlier
        {
            //localPCPlayer.SetActive(true);//activate local PC 
            //mainCamera.enabled = false;//turn off the main camera
            //mainCamera.SetActive(false);//turn off the main camera
            chooseGameAccess.switchScenes("Lobby");//Move player to lobby


        }
        else if (VRMode)//if VR button was clicked earlier
        {
            //localVRPlayer.SetActive(true);//activate local VR 
            //mainCamera.enabled = false;//turn off the main camera
            //mainCamera.SetActive(false);//turn off the main camera
            chooseGameAccess.switchScenes("Lobby");//Move player to lobby



        }
        //disactivate ability to go to beer pong
    }





    //--------------LAN Network buttons--------------------

    public void startHost()
    {
        audioManager.PlaySFX(audioManager.gmaeSetup_buttonClick);//play button click SFX when pressed

        NetworkManager.Singleton.StartHost();//start host
        gameSteupCanvas.SetActive(false );//hide net connect panel
        Debug.Log("Host started LAN");

        
        chooseGameAccess.switchScenesNetServerRpc("Lobby");

        staticClass.LANOn = true;


    }

    public void startClient()
    {
        audioManager.PlaySFX(audioManager.gmaeSetup_buttonClick);//play button click SFX when pressed

        //IPAdressText.SetActive(false);//hides ip address if not already
        NetworkManager.Singleton.StartClient();//join game as client
        gameSteupCanvas.SetActive(false);//hide net connect panel
        Debug.Log("Client started LAN");



    }

   

}
