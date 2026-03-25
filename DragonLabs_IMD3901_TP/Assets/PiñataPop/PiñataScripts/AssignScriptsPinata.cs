using Unity.Netcode;
using UnityEngine;

public class AssignScriptsPinata : NetworkBehaviour
{
    public static AssignScriptsPinata assigner;

    public NetworkObject scoresManagerObj;
    //private NetworkObject winBoardObj;

    private void Awake()
    {
        if (assigner == null)
        {
            assigner = this;
        }
    }

    public override void OnNetworkSpawn()
    {
        //get the network object of the scoresManager
        scoresManagerObj.GetComponent<NetworkObject>();
        //winBoardObj.GetComponent<NetworkObject>();
    }


    private void Update()
    {
        if(GameObject.Find("WinnerBoard(Clone)"))
        {
            Debug.Log("winner board has been spawned and successfully found");
        }

        /*if (winBoardObj.IsSpawned)
        {
            
            Debug.Log("winboard is spawned");
            winBoardObj.GetComponent<NetworkObject>();

            if (scoresManagerObj != null && winBoardObj != null)
            {
                //give the scoresManager field on the winBoard access to the scoresManager script when it spawns
                winBoardObj.GetComponent<WinnerManagerNet>().scoresManagerNet_access = scoresManagerObj.GetComponent<ScoresManagerNet>();
                Debug.Log("winboard field has been FILLED");
            }
        }*/

        
    }


}
