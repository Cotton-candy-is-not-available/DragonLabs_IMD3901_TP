using Unity.Netcode;
using UnityEngine;

public class AssignScriptsPinata : NetworkBehaviour
{
    public static AssignScriptsPinata assigner;

    public NetworkObject scoresManagerObj;

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

            //store the found spawned instance of the winboard in a new gameObject
            GameObject winBoardObj = GameObject.Find("WinnerBoard(Clone)");

            //give the scoresManager field on the winBoard access to the scoresManager script when it spawns
            winBoardObj.GetComponent<WinnerManagerNet>().scoresManagerNet_access = scoresManagerObj.GetComponent<ScoresManagerNet>();
            Debug.Log("winboard field has been FILLED");
        }
    }


}
