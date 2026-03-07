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
    //and websocket bool option should be checked meaning = True

    //This script can be on an empty game object
    //is currently on a canvas

    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField codeJoin;

    public GameObject joinPanel;

    [SerializeField] TextMeshProUGUI joinCodeDisplay;

    //public async Task<string> StartHostWithRelay(int maxConnections, string connectionType)
    //{
    //    await UnityServices.InitializeAsync();//connect to unity services
    //    if (!AuthenticationService.Instance.IsSignedIn)
    //    {
    //        Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);//get player ID
    //        await AuthenticationService.Instance.SignInAnonymouslyAsync();//creates an account for the user anonymously
    //    }
    //    var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
    //    NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "connectionType"));
    //
    //
    //
    //    var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    //    return NetworkManager.Singleton.StartHost() ? joinCode : null;
    //}

    //These functions get called when the host or client start
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
        await UnityServices.InitializeAsync();//initialises unity services so that API and relay can run
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();//creates account for user anonymously

    }


    //public async void createRelay()
    //{
    //        try
    //        {
    //            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);//max number of players
    //            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    //        }
    //        catch (RelayServiceException ex)
    //        {
    //            Debug.Log("RelayServiceException: " + ex);
    //        }
        
    //}

    //Same as create host button
    private async void createRelay()//creates an instance of relay so that user can connect to online service
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);//number of people who can join

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);//get join code

            Debug.Log(joinCode);

            joinCodeDisplay.text = joinCode; //display join code on the screen

            //RelayServerData relayServerData = new RelayServerData(allocation, "wss");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "wss"));//web socket connection type

            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            joinPanel.SetActive(false);//hide the join panel

        }
        catch (RelayServiceException err)
        {
            Debug.Log(err);
        }
    }

    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // RelayServerData relayServerData = new RelayServerData(joinAllocation, "wss");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(joinAllocation, "wss"));

            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            joinPanel.SetActive(false);//hide the join panel
        }
        catch (RelayServiceException err)
        {
            Debug.Log(err);
        }
    }


}
