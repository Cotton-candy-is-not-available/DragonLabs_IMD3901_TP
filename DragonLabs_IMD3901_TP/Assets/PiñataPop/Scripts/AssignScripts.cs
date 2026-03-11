//using Unity.Netcode;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;


public class AssignScripts : NetworkBehaviour
{
    public static AssignScripts assigner;

    public GameObject playerPCPrefab;
    public GameObject playerVRPrefab;

    public NetworkObject player1prefab;
    public NetworkObject player2prefab;

    //public GameObject AudioManager;

    public GameObject PinataController;
   

    private void Awake()
    {
        if (assigner == null)
        {
            assigner = this;
        }
    }

    public override void OnNetworkSpawn()
    {
        //get the network object of each player
        NetworkObject netObj = GetComponent<NetworkObject>();

        if (netObj.OwnerClientId == 0)
        {
            player1prefab = netObj; //host
        }
        else if (netObj.OwnerClientId == 1)
        {
            player2prefab = netObj; //client
        }
    }


    private void Update()
    {
        if (playerPCPrefab != null || playerVRPrefab != null)
        {
            //give PlayerInteractionNet field on the player prefabs access to the audio manager
            //playerPCPrefab.GetComponent<PlayerInteractionNet>().audioManager_access = AudioManager.GetComponent<AudioManager>();
            //playerVRPrefab.GetComponent<VRControllerInputNet>().audioManager_access = AudioManager.GetComponent<AudioManager>();

            //PinataController.GetComponent<NetworkPrefabs>().NetworkPrefabsLists = 

        }
    }

}
