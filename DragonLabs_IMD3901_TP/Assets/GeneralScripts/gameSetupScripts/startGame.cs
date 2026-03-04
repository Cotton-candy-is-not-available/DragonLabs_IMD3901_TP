using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class startGame : MonoBehaviour
{
    //bools will be called in other scenes to determine which prefabs and function need to be in use
    public bool VRMode = false;//need to make don't destroy onload
    public bool PCMode = false;//need to make don't destroy onload

    //need to make don't destroy onload
    public bool multiPlayerMode = false;
    public bool singlePlayerMode = false;

    //Panels to hide and show
    public GameObject startPanel;
    public GameObject plateformOptionPanel;
    public GameObject gameModeOptionPanel;
    public GameObject networkConnectPanel;

    //networkmanagers to activate
    public GameObject PCNetworkManager;
    public GameObject VRNetworkManager;

    public GameObject IPAdressText;


    private void Start()
    {
        //turn on start game panel by default
        startPanel.SetActive(true);//show start panel 

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
        multiPlayerMode = true;//player chose to play with unity networking
        gameObject.SetActive(false);//hides canvas

        networkConnectPanel.SetActive(true);//show network option(host, client, server)

        if (PCMode)//if pc button was clicked earlier
        {
            PCNetworkManager.SetActive(true);//activate PC network manager

        }
        else if (VRMode)//if VR button was clicked earlier
        {
            VRNetworkManager.SetActive(true);//activate VR network manager

        }
    }

    //If user chooses the single player option
    public void singlePlayerOptionButton()
    {
        gameModeOptionPanel.SetActive(false);//hides choose game mode panel on click
        singlePlayerMode = true;//player chose to play by themselves without networking

        gameObject.SetActive(false);//hides canvas

        //turn on local player prefabs and disable ability to go to beer pong

        //if vr
        //turn on XR manager
    }





    //--------------Network buttons--------------------


    public void startServer()
    {
        IPAdressText.SetActive(true);//shows ip address to connect to
        NetworkManager.Singleton.StartServer();//start server
    }

    public void startHost()
    {
        IPAdressText.SetActive(true);//shows ip address to connect to
        NetworkManager.Singleton.StartHost();//start host
    }

    public void startClient()
    {
        IPAdressText.SetActive(false);//hides ip address if not already
        NetworkManager.Singleton.StartHost();
    }





}
