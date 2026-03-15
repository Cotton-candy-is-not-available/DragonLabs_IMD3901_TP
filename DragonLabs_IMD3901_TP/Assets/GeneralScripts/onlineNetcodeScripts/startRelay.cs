using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;



public class startRelay : MonoBehaviour
{
    //On the network manager the unity transport objecs Protocol type dropt down
    //should be changed to relay unity transport instead of unity transport
    //and websocket bool option should be checked meaning = True (is done so in code or in inspector)
    //we are using web socket instead of dtls because some firewalls block dtls

    //This script can be on an empty game object
    //is currently on a canvas

     public Button hostButton;
     public Button joinButton;
     public  TMP_InputField codeJoin;

    public GameObject joinCanvas;

    [SerializeField] GameObject joinCodeDisplay;

    [SerializeField] int maxPlayerNum = 2;

    [SerializeField] hasStarted hasStartedAccesss;

    [SerializeField] startGame startGameAccesss;


    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            createRelay();
        });
        joinButton.onClick.AddListener(() =>
        {
            JoinRelay(codeJoin.text);
        });
    }


    private async void Start()
    {
        //if (hasStarted.gameHasStarted && !startGameAccesss.multiPlayerMode) return;//don't run relay authentication if game has already started and multiplayer mode is off
        if (hasStarted.gameHasStarted) return;//don't run relay authentication if game has already started and multiplayer mode is off

        await UnityServices.InitializeAsync();//initialises unity services so that API and relay can run
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);//get player ID
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();//creates account for user anonymously

        hasStarted.gameHasStarted = true;//set to true so that wehn the player comes bakc in the scene this fruntion does not run again

    }

    //Same as create host button
    public async void createRelay()//creates an instance of relay so that user can connect to online service
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayerNum);//number of people who can join

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);//get join code

            Debug.Log(joinCode);

            joinCodeDisplay.SetActive(true);
            joinCodeDisplay.GetComponent<TextMeshProUGUI>().text = joinCode; //display join code on the screen
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "wss"));//change unity trapsnport protocol to use relay
            NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;//set websocket checkmark to true

            NetworkManager.Singleton.StartHost();
            joinCanvas.SetActive(false);//hide the join panel

        }
        catch (RelayServiceException err)
        {
            Debug.Log("Start host error: " + err);
        }
    }

    //same as client button
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(joinAllocation, "wss"));//change unity trapsnport protocol to use relay
            NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;//set websocket checkmark to true

            NetworkManager.Singleton.StartClient();
            joinCanvas.SetActive(false);//hide the join panel
            //gameSetUpCanvas.SetActive(false);//hide the set up cnavas

        }
        catch (RelayServiceException err)
        {
            Debug.Log("Join error: " + err);
            throw;
        }
    }


}
