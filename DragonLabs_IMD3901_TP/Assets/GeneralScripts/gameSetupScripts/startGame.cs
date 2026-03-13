using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class startGame : MonoBehaviour
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


    //networkmanager to activate
    public GameObject NetworkManagerObject;

    public GameObject IPAdressText;
    public GameObject joinCodeText;


    private void Start()
    {
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


        if (PCMode)//if pc button was clicked earlier
        {
            localPCPlayer.SetActive(true);//activate local PC 

        }
        else if (VRMode)//if VR button was clicked earlier
        {
            localVRPlayer.SetActive(true);//activate local VR 

        }
       //disactivate ability to go to beer pong
    }





    //--------------LAN Network buttons--------------------


    public void startServer()
    {
        IPAdressText.SetActive(true);//shows ip address to connect to
        NetworkManager.Singleton.StartServer();//start server
        gameSteupCanvas.SetActive(false);//hide net connect panel

    }

    public void startHost()
    {
        IPAdressText.SetActive(true);//shows ip address to connect to
        NetworkManager.Singleton.StartHost();//start host
        gameSteupCanvas.SetActive(false );//hide net connect panel
    }

    public void startClient()
    {
        IPAdressText.SetActive(false);//hides ip address if not already
        NetworkManager.Singleton.StartClient();//join game as client
        gameSteupCanvas.SetActive(false);//hide net connect panel

    }




}
