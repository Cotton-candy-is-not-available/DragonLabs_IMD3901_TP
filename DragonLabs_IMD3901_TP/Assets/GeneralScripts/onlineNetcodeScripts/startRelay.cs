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

    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField codeJoin;

    public GameObject joinPanel;

    [SerializeField] TextMeshProUGUI joinCodeDisplay;

    [SerializeField] int maxPlayerNum = 2;

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
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);//get player ID
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();//creates account for user anonymously

    }

    //Same as create host button
    private async void createRelay()//creates an instance of relay so that user can connect to online service
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayerNum);//number of people who can join

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);//get join code

            Debug.Log(joinCode);

            joinCodeDisplay.text = joinCode; //display join code on the screen
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "wss"));//change unity trapsnport protocol to use relay
            NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;//set websocket checkmark to true

            NetworkManager.Singleton.StartHost();
            joinPanel.SetActive(false);//hide the join panel
            //gameSetUpCanvas.SetActive(false);//hide the set up cnavas


        }
        catch (RelayServiceException err)
        {
            Debug.Log("Start host error: " + err);
        }
    }

    //same as client button
    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(joinAllocation, "wss"));//change unity trapsnport protocol to use relay
            NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;//set websocket checkmark to true

            NetworkManager.Singleton.StartClient();
            joinPanel.SetActive(false);//hide the join panel
            //gameSetUpCanvas.SetActive(false);//hide the set up cnavas

        }
        catch (RelayServiceException err)
        {
            Debug.Log("Join error: " + err);
            throw;
        }
    }


}
